using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected enum State 
    {
        NONE,
        MOVE,
        STAY,
    }

    [SerializeField]
    int m_ID = -1;

    [SerializeField]
    int m_MaxHP = 1;
    int m_HP = 1;

    [SerializeField]
    float m_MoveSpeed = 5.0f;

    [SerializeField]
    int m_DeadEffectID = -1;

    [SerializeField]
    int m_SEDeadID = -1;

    [SerializeField]
    Transform m_DeadEffectNode = null;

    protected State m_State = State.NONE;
    Vector3 m_TargetPos = Vector3.zero;

    // プロパティ
    public int ID { get { return m_ID; } }
    public Vector3 TargetPos { get { return m_TargetPos; } set { m_TargetPos = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (m_State)
        {
            case State.MOVE:
                UpdateMove();
                break;

            default:
                break;
        }
    }

    void UpdateMove()
    {
        Vector3 beforePos = transform.position;

        // ターゲットに向かって移動
        Vector3 move = m_TargetPos - transform.position;
        move = move.normalized * m_MoveSpeed * Time.deltaTime;
        transform.position += move;

        // ターゲットを通り越したら位置固定して待機
        Vector3 moveDir = transform.position - beforePos;
        Vector3 toTarget = m_TargetPos - transform.position;
        if (Vector3.Dot(moveDir, toTarget) < 0)
        {
            transform.position = m_TargetPos;
            m_State = State.STAY;
        }
    }

    public virtual void Spawn(Vector3 pos, Quaternion rot)
    {
        m_State = State.MOVE;
        gameObject.SetActive(true);
        transform.position = pos;
        transform.rotation = rot;
        m_TargetPos = pos;
        m_HP = m_MaxHP;
    }

    private void Damage(int damage)
    {
        m_HP -= damage;
        // HP0以下で死亡
        if (m_HP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        // 死亡演出
        Transform trans = m_DeadEffectNode ? m_DeadEffectNode : transform;
        EffectManager.Instance.PlayEffect(m_DeadEffectID, trans.position);
        AudioManager.Instance.PlaySE(m_SEDeadID);

        // 倒したエネミー数加算
        PlayScene.Instance.KillEnemy++;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーの弾丸が当たった
        if (other.CompareTag("PlayerBullet"))
        {
            // ダメージ
            Bullet bullet = other.GetComponent<Bullet>();
            Damage(bullet.Damage);
        }
    }
}
