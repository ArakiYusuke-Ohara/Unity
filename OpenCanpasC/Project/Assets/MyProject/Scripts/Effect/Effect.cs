using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    int m_ID = -1;

    ParticleSystem m_ParticleSystem = null;

    public int ID { get { return m_ID; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!m_ParticleSystem.IsAlive(true))
        {
            gameObject.SetActive(false);
        }
        
    }

    public void Play(Vector3 pos)
    {
        gameObject.SetActive(true);
        m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        transform.position = pos;
        m_ParticleSystem.Play();
    }

}
