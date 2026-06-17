using UnityEngine;

public class AudioManager : MonoBehaviour
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
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }
    public void PlaySFX(AudioClip sfxclip)
    {
        sfxAudioSource.clip = sfxclip;
        sfxAudioSource.PlayOneShot(sfxclip);
    }
}
