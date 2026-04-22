using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BulletID = BulletManager.BulletID;
using EffectID = EffectManager.EffectID;
using SoundEffectID = AudioManager.SoundEffectID;

public class Player : MonoBehaviour
{
    [SerializeField]
    float m_Speed = 1.0f;

    [SerializeField]
    BulletID m_BulletID = BulletID.NONE;

    [SerializeField]
    EffectID m_DeadEffectID = EffectID.NONE;
    [SerializeField]
    SoundEffectID m_SEDeadID = SoundEffectID.NONE;

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
    SoundEffectID m_SELevelUpID = SoundEffectID.NONE;

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

        // 体験① 上移動入力処理





        // 体験② 他方向移動入力処理














        // 移動
        pos += move * m_Speed * Time.deltaTime;

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
        // 体験③ 攻撃ボタン入力判定
        if ( false )
        {
            if (m_BulletIntervalTimer <= 0.0f)
            {
                Vector3 pos = m_MuzzleLV1.position;
                Vector3 dir = m_MuzzleLV1.forward;

                // 体験④弾丸発射処理


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
        // 体験⑪ 死亡エフェクトを再生


        // 体験⑫ 死亡SEを再生


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
