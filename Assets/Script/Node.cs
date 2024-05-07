using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable; // Biến xác định Node có thể đi qua hay không
    public Vector3 worldPosition; // Vị trí của Node trong thế giới 3D
    public int gridX; // Vị trí x của Node trong grid
    public int gridY; // Vị trí y của Node trong grid

    public int gCost; // Chi phí từ điểm bắt đầu đến Node hiện tại
    public int hCost; // Chi phí từ Node hiện tại đến điểm đích
    public Node parent; // Node cha của Node hiện tại, dùng để tìm đường đi

    // Khởi tạo Node với các thông tin cơ bản
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // Tính toán tổng chi phí (gCost + hCost)
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
