using UnityEngine;

public class Ghoul_SoundAnimFrame : MonoBehaviour
{
    public AudioSource FootstepAudioSource;

    public void GhoulFootstep()
    {
        if (FootstepAudioSource != null)
        {
            FootstepAudioSource.Play();
        }
    }
}