using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    enum State 
    {
        NONE,
        MOVE,
        STAY,
    }

    [SerializeField]
    int m_ID = -1;

    [SerializeField]
    int m_HP = 1;

    [SerializeField]
    float m_MoveSpeed = 5.0f;

    State m_State = State.NONE;
    Vector3 m_TargetPos = Vector3.zero;

    // プロパティ
    public int ID { get { return m_ID; } }
    public Vector3 TargetPos { get { return m_TargetPos; } set { m_TargetPos = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
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

    public void Spawn(Vector3 pos, Quaternion rot)
    {
        m_State = State.MOVE;
        gameObject.SetActive(true);
        transform.position = pos;
        transform.rotation = rot;
        m_TargetPos = pos;
    }
}
