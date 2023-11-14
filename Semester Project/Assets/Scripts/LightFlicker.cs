using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight;           // The light to be turned on and off
    public GameObject emissionObject;   // The GameObject with emission texture

    public float minTimeOn = 0.25f;        // Minimum time the light is on or off (in seconds)
    public float maxTimeOn = 60f;          // Maximum time the light is on or off (in seconds)

    public float minTimeOff = 0.25f;        // Minimum time the light is on or off (in seconds)
    public float maxTimeOff = 4f;          // Maximum time the light is on or off (in seconds)

    private void Start()
    {
        // Start the routine to control the light
        StartCoroutine(RandomLightRoutine());
    }

    IEnumerator RandomLightRoutine()
    {
        while (true)
        {
            // Turn on the light
            targetLight.enabled = true;

            // If the emissionObject is not null, turn on its emission texture
            if (emissionObject != null)
            {
                Renderer renderer = emissionObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    material.EnableKeyword("_EMISSION");
                }
            }

            yield return new WaitForSeconds(Random.Range(minTimeOn, maxTimeOn));

            // Turn off the light
            targetLight.enabled = false;

            // If the emissionObject is not null, turn off its emission texture
            if (emissionObject != null)
            {
                Renderer renderer = emissionObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    material.DisableKeyword("_EMISSION");
                }
            }

            yield return new WaitForSeconds(Random.Range(minTimeOff, maxTimeOff));
        }
    }
}