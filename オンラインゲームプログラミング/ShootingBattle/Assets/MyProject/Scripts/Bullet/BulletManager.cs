using UnityEngine;
using System.Collections.Generic;
using Fusion;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    NetworkObject m_BulletPrafab = null;


    List<Bullet> m_Bullets = new List<Bullet>();

    NetworkRunner m_Runner = null;

    public static BulletManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init(NetworkRunner runner)
    {
        m_Runner = runner;
    }

    public Bullet FireBullet(Vector3 pos, Quaternion rot)
    {
        foreach (Bullet bullet in m_Bullets)
        {
            // 使われていない弾丸を再利用
            if (!bullet.IsActive)
            {
                bullet.Fire(pos, rot);
                return bullet;
            }
        }

        // 使いまわせない場合は弾丸をスポーンして発射
        NetworkObject bulletObj = m_Runner.Spawn(m_BulletPrafab);
        Bullet newBullet = bulletObj.GetComponent<Bullet>();
        newBullet.Fire(pos, rot);
        m_Bullets.Add(newBullet);

        return newBullet;
    }

    /// <summary>
    /// Spawnした弾丸を削除(Despawn)する
    /// もう弾丸が不要になるタイミングで呼ぶ
    /// </summary>
    public void DespawnAllBullet()
    {
        foreach (Bullet bullet in m_Bullets)
        {
            m_Runner.Despawn(bullet.Object);
        }
    }
}
