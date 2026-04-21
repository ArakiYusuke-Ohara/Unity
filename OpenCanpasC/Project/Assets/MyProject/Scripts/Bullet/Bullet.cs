using UnityEngine;
using BulletID = BulletManager.BulletID;
using EffectID = EffectManager.EffectID;
using SoundEffectID = AudioManager.SoundEffectID;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    BulletID m_ID = BulletID.NONE;

    [SerializeField]
    float m_Speed = 1.0f;

    [SerializeField]
    int m_Damage = 1;

    [SerializeField]
    private float m_Life = 1.0f;
    private float m_LifeCounter = 0.0f;

    [SerializeField]
    EffectID m_HitEffectID = EffectID.NONE;

    [SerializeField]
    SoundEffectID m_SEFireID = SoundEffectID.NONE;

    [SerializeField]
    SoundEffectID m_SEHitID = SoundEffectID.NONE;

    private Vector3 m_Move = Vector3.zero;


    // プロパティ
    public BulletID ID{ get { return m_ID; }}
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
        // 体験⑦ ヒットエフェクトを再生
        EffectManager.Instance.PlayEffect(m_HitEffectID, transform.position);

        // 体験⑧ ヒット音を再生
        AudioManager.Instance.PlaySE(m_SEHitID);

        gameObject.SetActive(false);
    }
}
