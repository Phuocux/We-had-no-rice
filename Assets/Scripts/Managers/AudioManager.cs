using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip musicClip;
    public AudioClip coinClip;
    public AudioClip healthClip;
    public AudioClip staminaClip;
    public AudioClip sword_AttackSFX;
    public AudioClip bow_AttackSFX;
    public AudioClip staff_AttackSFX;
    public AudioClip playerHurtSFX;
    public AudioClip gun_AttackSFX;
    public AudioClip playerDeathSFX;
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }
    public void PlaySFX(AudioClip sfxclip)
    {
        if (sfxAudioSource == null)
        {
            Debug.LogError("SFX AudioSource đã bị mất!");
            return;
        }

        sfxAudioSource.PlayOneShot(sfxclip);
    }
}
