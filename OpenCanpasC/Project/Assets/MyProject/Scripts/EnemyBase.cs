using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    int m_ID = -1;

    [SerializeField]
    int m_HP = 1;

    Vector3 m_TargetPos = Vector3.zero;

    // プロパティ
    public int ID { get { return m_ID; } }
    public Vector3 TargetPos { get { return m_TargetPos; } set { m_TargetPos = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Vector3 pos, Quaternion rot)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        transform.rotation = rot;
    }
}
