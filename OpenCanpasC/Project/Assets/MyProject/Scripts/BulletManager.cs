using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // ID順に設定する弾丸の元データ
    [SerializeField]
    private GameObject[] m_BulletList;

    // 発射された弾丸リスト
    private List<Bullet> m_Bullets = new List<Bullet>();

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBullet(int id, Vector3 pos, Vector3 move)
    {
        // IDチェック
        if (id < 0) return;

        // 発射リストを見て同じ弾丸かつ未使用のものがあれば再利用
        foreach (Bullet bullet in m_Bullets)
        {
            if (bullet.ID == id && !bullet.Active)
            {
                bullet.Fire(pos, move);
                return;
            }
        }

        // 再利用できなければ生成して発射
        GameObject obj = Instantiate(m_BulletList[id]);
        Bullet bulletComponent = obj.GetComponent<Bullet>();
        bulletComponent.Fire(pos, move);
    }
}
