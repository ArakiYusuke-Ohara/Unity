using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int m_ID = -1;

    [SerializeField]
    private float m_Life = 1.0f;
    private float m_LifeCounter = 0.0f;
    private Vector3 m_Move = Vector3.zero;

    // プロパティ
    public int ID{ get { return m_ID; }}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 移動量通りに飛んでいくだけ
        transform.position += m_Move * Time.deltaTime;

        // 寿命処理
        m_LifeCounter -= Time.deltaTime;
        if (m_LifeCounter <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Fire(Vector3 pos, Vector3 move)
    {
        m_LifeCounter = m_Life;
        gameObject.SetActive(true);
        transform.position = pos;
        m_Move = move;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵にヒット
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
