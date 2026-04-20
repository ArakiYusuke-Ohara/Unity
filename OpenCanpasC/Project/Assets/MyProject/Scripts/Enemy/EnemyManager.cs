using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    const int LOCATION_NUMBER_MAX = 27;
    const int ROW_NUM = 7;
    const int COL_NUM = 4;
    const float ROW_GAP = 5.0f;
    const float COL_GAP = 8.0f;
    const float LOCATION_LEFT = -2.0f;
    const float LOCATION_BOTTOM = -20.0f;
    const float START_X = 45.0f;
    const int LEVEL_MAX = 10;

    struct LevelParameter
    {
        public float spawnInterval;
        public int spawnNum;
    }

    LevelParameter[] m_LevelParam =
    {
        new LevelParameter { spawnInterval = 5.0f, spawnNum = 1 },
        new LevelParameter { spawnInterval = 4.0f, spawnNum = 1 },
        new LevelParameter { spawnInterval = 3.0f, spawnNum = 1 },
        new LevelParameter { spawnInterval = 5.0f, spawnNum = 2 },
        new LevelParameter { spawnInterval = 4.0f, spawnNum = 2 },
        new LevelParameter { spawnInterval = 3.0f, spawnNum = 2 },
        new LevelParameter { spawnInterval = 5.0f, spawnNum = 3 },
        new LevelParameter { spawnInterval = 4.0f, spawnNum = 3 },
        new LevelParameter { spawnInterval = 3.0f, spawnNum = 3 },
        new LevelParameter { spawnInterval = 4.0f, spawnNum = 5 },
    };

    [SerializeField]
    private GameObject[] m_EnemyMasterObject;

    private List<EnemyBase> m_Enemis = new List<EnemyBase>();

    // ランダムに並べる配置番号
    List<int> locationNumbers = new List<int>();
    int locationIndex = 0;

    // スポーンインターバル
    float m_SpawnInterval = 5.0f;
    float m_SpawnIntervalTimer = 0.0f;

    // レベルアップインターバル
    [SerializeField]
    float m_LevelUpInterval = 20.0f;
    float m_LevelUpIntervalTimer = 0.0f;

    int m_Level = 0;

    public static EnemyManager Instance { get; private set; }

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
        // 0～27を入れる
        for (int i = 0; i <= 27; i++)
        {
            locationNumbers.Add(i);
        }

        // ロケーション番号をシャッフル
        ShuffleLocationNumbers();

        // 初期値設定
        m_SpawnInterval = m_LevelParam[0].spawnInterval;
        m_SpawnIntervalTimer = 0.0f;
        m_LevelUpIntervalTimer = m_LevelUpInterval;
        m_Level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // レベルアップ更新処理
        UpdateLevelUp();
        // スポーン更新処理
        UpdateSpawner();
    }

    void UpdateLevelUp()
    {
        // 一定時間でレベルアップ
        if (m_LevelUpIntervalTimer <= 0.0f)
        {
            LevelUp();
            m_LevelUpIntervalTimer = m_LevelUpInterval;
        }
        m_LevelUpIntervalTimer -= Time.deltaTime;
    }

    void LevelUp()
    {
        m_Level++;
        if (m_Level >= LEVEL_MAX) return;

        m_SpawnInterval = m_LevelParam[m_Level].spawnInterval;
    }

    void UpdateSpawner()
    {
        // 一定時間でスポーン
        if (m_SpawnIntervalTimer <= 0.0f)
        {
            // レベルによって同時スポーン数が変わる
            int spawnNum = m_LevelParam[m_Level].spawnNum;
            for (int i = 0; i < spawnNum; i++)
            {
                RandomLocationSpawn();
            }

            m_SpawnIntervalTimer = m_SpawnInterval;
        }
        m_SpawnIntervalTimer -= Time.deltaTime;
    }

    EnemyBase SpawnEnemy(int id, Vector3 pos, Quaternion rot)
    {
        // IDチェック
        if (id < 0) return null;

        // 発射リストを見て同じ弾丸かつ未使用のものがあれば再利用
        foreach (EnemyBase enemy in m_Enemis)
        {
            if (enemy.ID == id && !enemy.gameObject.activeInHierarchy)
            {
                enemy.Spawn(pos, rot);
                return enemy;
            }
        }

        // 再利用できなければ生成してスポーン
        GameObject obj = Instantiate(m_EnemyMasterObject[id]);
        EnemyBase enemyComponent = obj.GetComponent<EnemyBase>();
        enemyComponent.Spawn(pos, rot);
        m_Enemis.Add(enemyComponent);

        return enemyComponent;
    }

    /// <summary>
    /// 配置番号リストをシャッフルする
    /// </summary>
    void ShuffleLocationNumbers()
    {
        for (int i = 0; i < locationNumbers.Count; i++)
        {
            int rand = Random.Range(i, locationNumbers.Count);
            int temp = locationNumbers[i];
            locationNumbers[i] = locationNumbers[rand];
            locationNumbers[rand] = temp;
        }

        locationIndex = 0;
    }

    /// <summary>
    /// ランダム配置でスポーン
    /// </summary>
    void RandomLocationSpawn()
    {
        int number = locationNumbers[locationIndex];
        int x = number % COL_NUM;
        int y = number % ROW_NUM;
        float targetX = x * COL_GAP + LOCATION_LEFT;
        float targetY = y * ROW_GAP + LOCATION_BOTTOM;
        EnemyBase enemy = SpawnEnemy(0, new Vector3(START_X, targetY, 0.0f), Quaternion.Euler(0.0f, 270.0f, 0.0f));
        enemy.TargetPos = new Vector3(targetX, targetY, 0.0f);
        locationIndex++;

        if(locationIndex > LOCATION_NUMBER_MAX)
        {
            locationIndex = 0;
        }
    }
}
