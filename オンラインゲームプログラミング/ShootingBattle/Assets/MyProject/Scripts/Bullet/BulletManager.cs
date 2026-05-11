using UnityEngine;
using System.Collections.Generic;
using Fusion;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    NetworkObject m_BulletPrafab = null;


    List<Bullet> m_Bullets = new List<Bullet>();

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

    public Bullet FireBullet(NetworkRunner runner, Vector3 pos, Quaternion rot)
    {
        // オブジェクトプールはプール内を同期する必要があるので
        // とても難しい

        // 弾丸をスポーンして発射
        NetworkObject bulletObj = runner.Spawn(m_BulletPrafab);
        Bullet bulletComp = bulletObj.GetComponent<Bullet>();
        bulletComp.Fire(pos, rot);

        return bulletComp;
    }
}
