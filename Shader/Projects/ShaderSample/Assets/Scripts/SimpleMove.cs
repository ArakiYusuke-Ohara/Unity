using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField]
    float m_MoveSpeed = 5.0f;

    [SerializeField]
    float m_RotSpeed = 200.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move.y = 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move.y = -1.0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.x = -1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move.x = 1.0f;
        }

        Vector3 axis = Vector3.zero;
        float rotDir = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            axis = Vector3.right;
            rotDir = 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            axis = Vector3.up;
            rotDir = 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            axis = Vector3.right;
            rotDir = -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            axis = Vector3.up;
            rotDir = -1.0f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            axis = Vector3.forward;
            rotDir = 1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            axis = Vector3.forward;
            rotDir = -1.0f;
        }

        transform.Translate(move.normalized * m_MoveSpeed * Time.deltaTime);
        transform.Rotate(axis, m_RotSpeed * rotDir * Time.deltaTime);
    }
}
