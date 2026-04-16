using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float m_Speed = 1.0f;

    [SerializeField]
    int m_BulletID = -1;

    [SerializeField]
    float m_BulletSpeed = 50.0f;


    Rigidbody m_Rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();     // ˆÚ“®ˆ—
        Attack();   // UŒ‚ˆ—
    }

    private void Move()
    {
        Vector3 pos = transform.position;
        Vector3 move = Vector3.zero;

        // ˆÚ“®“ü—Íˆ—
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move.y = m_Speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move.y = -m_Speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.x = -m_Speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move.x = m_Speed;
        }

        // ˆÚ“®
        pos += move * Time.deltaTime;

        // ‰æ–Ê‚©‚ço‚È‚¢‚æ‚¤‚É
        pos.x = Mathf.Clamp(pos.x, -20.0f, 1.0f);
        pos.y = Mathf.Clamp(pos.y, -14.0f, 12.0f);
        transform.position = pos;
    }

    private void Attack()
    {
        // ’eŠÛˆ—
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // ”­Ë
            BulletManager.Instance.FireBullet(m_BulletID, transform.position, new Vector3(m_BulletSpeed, 0.0f, 0.0f));
        }
    }
}
