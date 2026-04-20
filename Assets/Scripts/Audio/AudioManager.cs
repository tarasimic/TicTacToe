using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip buttonClickClip;
    public AudioClip placementClip;
    public AudioClip winClip;
    public AudioClip popupClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();
        bgmSource = sources[0];
        sfxSource = sources[1];
        bgmSource.loop = true;
    }

    private void Start()
    {
        bgmSource.mute = !GameManager.Instance.gameData.bgmEnabled;
        sfxSource.mute = !GameManager.Instance.gameData.sfxEnabled;
        PlayBGM();
    }

    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && !sfxSource.mute)
            sfxSource.PlayOneShot(clip);
    }

    public void SetBGMEnabled(bool value)
    {
        bgmSource.mute = !value;
        GameManager.Instance.gameData.bgmEnabled = value;
        GameManager.Instance.SaveGame();
    }

    public void SetSFXEnabled(bool value)
    {
        sfxSource.mute = !value;
        GameManager.Instance.gameData.sfxEnabled = value;
        GameManager.Instance.SaveGame();
    }
}