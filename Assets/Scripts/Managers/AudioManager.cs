using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    [Header("Clips")]
    public AudioClip BackgroundSound;
    public AudioClip PowerupPickupSound;
    public AudioClip ShootSound;
    public AudioClip HitSound;
    public AudioClip MutationSound;
    public AudioClip StoneKnockbackSound;
    public AudioClip VeilInvulSound;
    public AudioClip BladeDashSound;
    public AudioClip GameOverSound;
    public AudioClip ButtonSound;

    public static AudioManager Instance { get; private set; }

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

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        MusicSource.PlayOneShot(clip);
    }
}
