using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip hurtSFX;
    [SerializeField] private AudioClip deathSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttack()
    {
        if (attackSFX != null)
            audioSource.PlayOneShot(attackSFX);
    }

    public void PlayHurt()
    {
        if (hurtSFX != null)
            audioSource.PlayOneShot(hurtSFX);
    }

    public void PlayDeath()
    {
        if (deathSFX != null)
            audioSource.PlayOneShot(deathSFX);
    }
}