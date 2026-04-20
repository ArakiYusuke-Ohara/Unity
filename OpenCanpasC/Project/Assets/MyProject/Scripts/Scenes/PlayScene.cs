using UnityEngine;

public class PlayScene : MonoBehaviour
{
    [SerializeField]
    int m_BGM_ID = -1;

    int m_KillEnemy = 0;

    public int KillEnemy { get { return m_KillEnemy; } set { m_KillEnemy = value; } }

    public static PlayScene Instance { get; private set; }

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
        AudioManager.Instance.PlayBGM(m_BGM_ID);
        m_KillEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
