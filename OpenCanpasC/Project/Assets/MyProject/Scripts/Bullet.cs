using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int m_ID = -1;

    private bool m_Active = false;
    private Vector3 m_Move = Vector3.zero;

    // プロパティ
    public int ID{ get { return m_ID; }}
    public bool Active { get { return m_Active; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Active) return;

        // 移動量通りに飛んでいくだけ
        transform.position += m_Move * Time.deltaTime;
    }

    public void Fire(Vector3 pos, Vector3 move)
    {
        m_Active = true;
        transform.position = pos;
        m_Move = move;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵にヒット
        if (other.CompareTag("Enemy"))
        {
            m_Active = false;
        }
    }
}
