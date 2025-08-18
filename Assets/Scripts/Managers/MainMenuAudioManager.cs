using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource MenuMusicSource;
    public AudioSource MenuSFXSource;

    [Header("Clips")]
    public AudioClip MenuMusic;
    public AudioClip ButtonSound;
    public static MainMenuAudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayMenuMusic(AudioClip clip)
    {
        MenuMusicSource.clip = clip;
        MenuMusicSource.loop = true;
        MenuMusicSource.Play();
    }

    public void PlayMenuSFX(AudioClip clip)
    {
        MenuSFXSource.PlayOneShot(clip);
    }
}
