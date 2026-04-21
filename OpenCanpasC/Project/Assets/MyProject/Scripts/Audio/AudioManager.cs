using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum BackGroundMusicID
    {
        NONE = -1,
        PLAY_SCENE,
    }

    public enum SoundEffectID
    {
        NONE = -1,
        FIRE_PLAYER_BULLET,
        FIRE_BEHOLDER_BULLET,
        DAMAGE_PLAYER,
        HIT_PLAYER_BULLET,
        HIT_BEHOLDER_BULLET,
        BEHOLDER_DEAD,
        LEVEL_UP_PLAYER,
        DEAD_PLAYER
    }

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

    public void PlayBGM(BackGroundMusicID id)
    {
        m_BGMSource.clip = m_BGMClips[(int)id];
        m_BGMSource.loop = true;
        m_BGMSource.Play();
    }

    public void PlaySE(SoundEffectID id)
    {
        if (id < 0 || (int)id >= m_SEClips.Length) return;

        m_SESource.PlayOneShot(m_SEClips[(int)id]);
    }
}
