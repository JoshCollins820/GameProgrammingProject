using UnityEngine;

public class SoundOnAnimationFrame : MonoBehaviour
{
    public AudioSource audioSource;

    public void EnemyFootstep()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}