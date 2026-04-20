using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    float m_Speed = 1.0f;

    [SerializeField]
    int m_BulletID = -1;

    [SerializeField]
    int m_DeadEffectID = -1;

    [SerializeField]
    float m_BulletInterval = 0.2f;

    [SerializeField]
    Transform m_Muzzle;

    float m_BulletIntervalTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_BulletIntervalTimer = 0.0f;
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
        pos.x = Mathf.Clamp(pos.x, -24.0f, -2.0f);
        pos.y = Mathf.Clamp(pos.y, -14.0f, 14.0f);
        transform.position = pos;
    }

    private void Attack()
    {
        // ’eŠÛˆ—
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_BulletIntervalTimer <= 0.0f)
            {
                // ”­Ë
                BulletManager.Instance.FireBullet(m_BulletID, m_Muzzle.position, m_Muzzle.forward);
                m_BulletIntervalTimer = m_BulletInterval;
            }
        }

        m_BulletIntervalTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("Result", LoadSceneMode.Additive);
        }
    }

}
