using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBeholder : EnemyBase
{
    [SerializeField]
    int m_BulletID = -1;

    [SerializeField]
    float m_BulletInterval = 3.0f;
    float m_BulletIntervalTimer = 0.0f;

    [SerializeField]
    Transform m_Mussle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_State == State.STAY)
        {
            UpdateAttack();
        }
    }

    void UpdateAttack()
    {
        if (m_BulletIntervalTimer <= 0.0f)
        {
            BulletManager.Instance.FireBullet(m_BulletID, m_Mussle.position, m_Mussle.forward);
            m_BulletIntervalTimer = m_BulletInterval;
        }

        m_BulletIntervalTimer -= Time.deltaTime;
    }

    public override void Spawn(Vector3 pos, Quaternion rot)
    {
        base.Spawn(pos, rot);

        m_BulletIntervalTimer = 0.0f;
    }
}
