using UnityEngine;
using static EffectManager;
using EffectID = EffectManager.EffectID;

public class Effect : MonoBehaviour
{
    [SerializeField]
    EffectID m_ID = EffectID.NONE;

    ParticleSystem m_ParticleSystem = null;

    public EffectID ID { get { return m_ID; } }

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
