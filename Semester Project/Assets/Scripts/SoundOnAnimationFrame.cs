using UnityEngine;

public class SoundOnAnimationFrame : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource PlayeraudioSource;

    public void EnemyFootstep()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}