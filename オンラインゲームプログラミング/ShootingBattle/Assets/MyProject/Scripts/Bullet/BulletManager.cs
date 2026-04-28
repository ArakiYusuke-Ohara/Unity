using UnityEngine;
using System.Collections.Generic;
using Fusion;

public class BulletManager : NetworkBehaviour
{
    [SerializeField]
    GameObject m_BulletPrafab = null;


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

    public Bullet FireBullet(Vector3 pos, Quaternion rot)
    {
        foreach (Bullet bullet in m_Bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.Fire(pos, rot);
                return bullet;
            }
        }

        NetworkObject bulletObj = Runner.Spawn(m_BulletPrafab);
        Bullet bulletComp = bulletObj.GetComponent<Bullet>();
        bulletComp.Fire(pos, rot);

        return bulletComp;
    }
}
