using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    GridScript grid;

    void Awake()
    {
        grid = GetComponent<GridScript>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // lấy vị trí của 2 điểm start và target tham chiếu tới grid node tương ứng
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); //các currentNode chưa được duyệt
        HashSet<Node> closedSet = new HashSet<Node>(); //các currentNode đã được duyệt
        openSet.Add(startNode); //Thêm nút bắt đầu vào Open

        while (openSet.Count > 0)
        {
            // Chọn nút hiện tại trong tập Open có f cost thấp nhất.
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            // Loại bỏ nút hiện tại khỏi tập Open và đưa vào tập Closed.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Nút hiện tại là nút kết thúc: Dừng thuật toán.
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            // Duyệt các nút xung quanh (neighbor) của nút hiện tại
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // Nếu:
                // +	Nút neighbor đã nằm trong tập Closed: Bỏ qua
                // +    Nút neighbor là chướng ngại vật: Bỏ qua
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }


                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); // khoảng cách mới từ currentNode -> neighborNode

                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // cập nhật fCost cho neighbor bằng set gCost và hCost
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode; // set currentNode là node cha của neighborNode

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
