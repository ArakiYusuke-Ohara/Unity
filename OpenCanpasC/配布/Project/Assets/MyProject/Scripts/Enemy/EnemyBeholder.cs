using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using BulletID = BulletManager.BulletID;

public class EnemyBeholder : EnemyBase
{
    [SerializeField]
    BulletID m_BulletID = BulletID.NONE;

    [SerializeField]
    float m_BulletInterval = 3.0f;
    float m_BulletIntervalTimer = 0.0f;

    [SerializeField]
    Transform m_Muzzle;

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
            BulletManager.Instance.FireBullet(m_BulletID, m_Muzzle.position, m_Muzzle.forward);
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
