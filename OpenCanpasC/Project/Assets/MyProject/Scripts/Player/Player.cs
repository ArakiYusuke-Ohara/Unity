using System.Collections;
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
    int m_SEDeadID = -1;

    [SerializeField]
    float m_BulletInterval = 0.2f;

    [SerializeField]
    Transform m_MuzzleLV1;

    [SerializeField]
    Transform []m_MuzzleLV2;

    [SerializeField]
    Transform[] m_MuzzleLV3;

    [SerializeField]
    int[] m_NextEXP;
    int m_EXP = 0;
    int m_Level = 0;

    [SerializeField]
    GameObject m_LevelUpText = null;

    [SerializeField]
    GameObject m_LevelUpEffect = null;
    ParticleSystem m_LevelUpParticle = null;

    [SerializeField]
    int m_SELevelUpID = -1;

    float m_BulletIntervalTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerManager.Instance.PlayerComponent = this;
        m_BulletIntervalTimer = 0.0f;
        m_LevelUpParticle = m_LevelUpEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();     // 移動処理
        Attack();   // 攻撃処理
    }

    private void Move()
    {
        Vector3 pos = transform.position;
        Vector3 move = Vector3.zero;

        // 移動入力処理
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

        // 移動
        pos += move * Time.deltaTime;

        // 画面から出ないように
        pos.x = Mathf.Clamp(pos.x, -24.0f, -2.0f);
        pos.y = Mathf.Clamp(pos.y, -16.0f, 14.0f);
        transform.position = pos;
    }

    private void Attack()
    {
        switch (m_Level)
        {
            case 0:
                AttackLV1();
                break;
            case 1:
                AttackLV2();
                break;
            case 2:
                AttackLV3();
                break;

        }
    }

    private void AttackLV1()
    {
        // 弾丸処理
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_BulletIntervalTimer <= 0.0f)
            {
                // 発射
                BulletManager.Instance.FireBullet(m_BulletID, m_MuzzleLV1.position, m_MuzzleLV1.forward);
                m_BulletIntervalTimer = m_BulletInterval;
            }
        }

        m_BulletIntervalTimer -= Time.deltaTime;
    }

    private void AttackLV2()
    {
        // 弾丸処理
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_BulletIntervalTimer <= 0.0f)
            {
                foreach (Transform muzzle in m_MuzzleLV2)
                {
                    // 発射
                    BulletManager.Instance.FireBullet(m_BulletID, muzzle.position, muzzle.forward);
                    m_BulletIntervalTimer = m_BulletInterval;
                }
            }
        }

        m_BulletIntervalTimer -= Time.deltaTime;
    }
    private void AttackLV3()
    {
        // 弾丸処理
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_BulletIntervalTimer <= 0.0f)
            {
                foreach (Transform muzzle in m_MuzzleLV3)
                {
                    // 発射
                    BulletManager.Instance.FireBullet(m_BulletID, muzzle.position, muzzle.forward);
                    m_BulletIntervalTimer = m_BulletInterval;
                }
            }
        }

        m_BulletIntervalTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            Dead();
            SceneManager.LoadScene("Result", LoadSceneMode.Additive);
        }
    }

    private void Dead()
    {
        EffectManager.Instance.PlayEffect(m_DeadEffectID, transform.position);
        AudioManager.Instance.PlaySE(m_SEDeadID);

        gameObject.SetActive(false);
    }

    public void AddExp(int exp)
    {
        m_EXP += exp;

        // レベルアップチェック
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        bool isLevelUp = false;
        // レベルを上がるところまで上げる
        while (m_Level < m_NextEXP.Length && m_EXP >= m_NextEXP[m_Level])
        {
            m_EXP -= m_NextEXP[m_Level];
            m_Level++;
            isLevelUp = true;
        }

        // レベルが上がったら演出
        if (isLevelUp)
        {
            AudioManager.Instance.PlaySE(m_SELevelUpID);
            StartCoroutine(LevelUpEffect());
        }
    }

    /// <summary>
    /// 1秒間LevelUpを表示するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator LevelUpEffect()
    {
        m_LevelUpEffect.SetActive(true);
        m_LevelUpText.SetActive(true);
        m_LevelUpParticle.Play();

        yield return new WaitForSeconds(1.0f);

        m_LevelUpText.SetActive(false);
        m_LevelUpEffect.SetActive(false);
    }
}
