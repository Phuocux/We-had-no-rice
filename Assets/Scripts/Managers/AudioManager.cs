using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip musicClip;
    public AudioClip coinClip;
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }
    public void PlaySFX(AudioClip sfxclip)
    {
        sfxAudioSource.clip = sfxclip;
        sfxAudioSource.PlayOneShot(sfxclip);
    }
}
