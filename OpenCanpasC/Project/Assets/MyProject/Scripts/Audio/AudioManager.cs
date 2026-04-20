using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_BGMSource = null;

    [SerializeField]
    AudioSource m_SESource = null;

    [SerializeField]
    AudioClip[] m_BGMClips;

    [SerializeField]
    AudioClip[] m_SEClips;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayBGM(int id)
    {
        m_BGMSource.clip = m_BGMClips[id];
        m_BGMSource.loop = true;
        m_BGMSource.Play();
    }

    public void PlaySE(int id)
    {
        if (id < 0 || id >= m_SEClips.Length) return;

        m_SESource.PlayOneShot(m_SEClips[id]);
    }
}
