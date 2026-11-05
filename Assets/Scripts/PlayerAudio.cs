using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip starSFX;
    [SerializeField] private AudioClip dieSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void PlayHitSound()
    {
        if (hitSFX != null)
        {
            audioSource.PlayOneShot(hitSFX);
        }
    }

    public void PlayStarSound()
    {
        if (starSFX != null)
        {
            audioSource.PlayOneShot(starSFX);
        }
    }

    public void PlayDieSound()
    {
        if (dieSFX != null)
        {
            audioSource.PlayOneShot(dieSFX);
        }
    }
}