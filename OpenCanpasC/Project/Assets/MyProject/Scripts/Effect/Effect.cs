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

    public void Play(Vector3 pos)
    {
        m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        transform.position = pos;
        m_ParticleSystem.Play();
    }

}
