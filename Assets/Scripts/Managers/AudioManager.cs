using UnityEngine;
using UnityEngine.UI;

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

    [Header("Mute Button Sprites")]
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

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
        SFXSource.PlayOneShot(clip);
    }

    public void ToggleMute()
    {
        MusicSource.mute = !MusicSource.mute;
        _buttonImage.sprite = MusicSource.mute ? _soundOffSprite : _soundOnSprite;
    }
}
