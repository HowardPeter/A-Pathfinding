using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform player, target; // Player và mục tiêu để tìm đường
    public LayerMask unwalkableMask; // Layer vật thể không thể đi qua (tường)
    public Vector2 gridWorldSize; // Kích thước của grid
    public float nodeRadius; // Bán kính của mỗi Node trong grid
    public bool pathOnly; // Chỉ hiển thị đường đi trên Gizmos hay không
    Node[,] grid; // Mảng 2 chiều của các Node trong grid

    float nodeDiameter; // Đường kính của mỗi Node
    int gridSizeX, gridSizeY; // Kích thước của grid trong các Node

    void Start()
    {
        // Tính toán đường kính của mỗi Node và kích thước của grid
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    // Hàm tạo grid
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY]; //Khởi tạo grid

        // Tính toán điểm bắt đầu của grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        // Vòng lặp để tạo Node cho mỗi ô trong grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius); // Tính toán vị trí của Node
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); // Kiểm tra xem Node có thể đi qua hay không bằng cách kiểm tra va chạm với các vật thể không thể đi qua

                grid[x, y] = new Node(walkable, worldPoint, x, y); // Gán Node vào grid
            }
        }
    }

    // Lấy các Node neighbor của một Node cho trước
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Duyệt qua các ô xung quanh Node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Bỏ qua Node hiện tại
                if (x == 0 && y == 0)
                    continue;

                // Tính toán vị trí x, y của Node láng giềng
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Kiểm tra xem Node láng giềng có nằm trong grid không, nếu có thì thêm Node vào danh sách neighbor
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    // Đối chiếu Node từ 1 vị trí cho trước
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Tính toán phần trăm vị trí của Node trong grid
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Tính toán vị trí x, y của Node trong grid
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;

    // Vẽ đường đi cho thuật toán và grid
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

        // Nếu pathOnly = true, chỉ hiển thị đường đi thuật toán, nếu pathOnly = false thì hiển thị bản đồ grid
        if (pathOnly)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    // Vẽ đường đi màu cyan
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {

            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    // Nếu Node là đường đi thì vẽ ô màu trắng, nếu là tường thì vẽ ô màu đen
                    Gizmos.color = (n.walkable) ? Color.white : Color.black;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}