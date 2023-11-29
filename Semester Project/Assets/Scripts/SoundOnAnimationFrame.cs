using UnityEngine;

public class SoundOnAnimationFrame : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource swingSource;

    public void EnemyFootstep()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void EnemySwing()
    {
        if (swingSource != null)
        {
            swingSource.Play();
        }
    }
}