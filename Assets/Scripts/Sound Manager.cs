using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip hurtSound;
    public AudioClip hitSound;

    // Cooldown settings
    private float hitCooldown = 1.0f; // Cooldown time in seconds for hit sound
    private float hurtCooldown = 1.0f; // Cooldown time in seconds for hurt sound
    private float lastHitTime = -Mathf.Infinity; // Track last time hit sound was played
    private float lastHurtTime = -Mathf.Infinity; // Track last time hurt sound was played

    // Singleton instance
    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayHitSound()
    {
        if (Time.time - lastHitTime >= hitCooldown)
        {
            sfxSource.PlayOneShot(hitSound);
            lastHitTime = Time.time;
        }
    }

    public void PlayHurtSound()
    {
        if (Time.time - lastHurtTime >= hurtCooldown)
        {
            sfxSource.PlayOneShot(hurtSound);
            lastHurtTime = Time.time;
        }
    }
}
