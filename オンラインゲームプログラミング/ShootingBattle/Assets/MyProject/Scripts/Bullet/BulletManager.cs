using UnityEngine;
using System.Collections.Generic;
using Fusion;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    NetworkObject m_BulletPrafab = null;

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
        // オブジェクトプールは再利用処理がかなり難しい

        // 使いまわせない場合は弾丸をスポーンして発射
        NetworkObject bulletObj = m_Runner.Spawn(m_BulletPrafab);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Fire(pos, rot);

        return bullet;
    }
}
