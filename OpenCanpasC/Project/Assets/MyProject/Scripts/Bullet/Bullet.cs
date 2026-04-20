using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int m_ID = -1;

    [SerializeField]
    float m_Speed = 1.0f;

    [SerializeField]
    int m_Damage = 1;

    [SerializeField]
    private float m_Life = 1.0f;
    private float m_LifeCounter = 0.0f;

    [SerializeField]
    int m_HitEffectID = -1;

    [SerializeField]
    int m_SEFireID = -1;

    [SerializeField]
    int m_SEHitID = -1;

    private Vector3 m_Move = Vector3.zero;


    // プロパティ
    public int ID{ get { return m_ID; }}
    public int Damage { get { return m_Damage; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 移動量通りに飛んでいくだけ
        transform.position += m_Move * m_Speed * Time.deltaTime;

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
        m_Move = move.normalized;

        // 発射音再生
        AudioManager.Instance.PlaySE(m_SEFireID);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーの弾丸が敵にヒット
        if (CompareTag("PlayerBullet") && other.CompareTag("Enemy"))
        {
            Hit();
        }

        // 敵の弾丸がプレイヤーにヒット
        else if (CompareTag("EnemyBullet") && other.CompareTag("Player"))
        {
            Hit();
        }

    }

    private void Hit()
    {
        // ヒットエフェクトを再生
        EffectManager.Instance.PlayEffect(m_HitEffectID, transform.position);

        // ヒット音再生
        AudioManager.Instance.PlaySE(m_SEHitID);

        gameObject.SetActive(false);
    }
}
