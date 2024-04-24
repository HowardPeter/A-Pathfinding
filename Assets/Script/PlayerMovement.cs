using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed;
    float horizontal, vertical;
    private float sposX = (float)-7.5;
    private float sposY = (float)-4.55;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(sposX, sposY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        GetDirection(horizontal, vertical);
    }
    void GetDirection(float h, float v)
    {
        if (h == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (h == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (v == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (v == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

}
