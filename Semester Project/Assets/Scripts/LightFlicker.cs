using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;

    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float minFlickerSpeed = 0.1f;
    public float maxFlickerSpeed = 1.0f;

    private float originalIntensity;

    private void Start()
    {
        if (flickeringLight == null)
        {
            flickeringLight = GetComponent<Light>();
        }

        if (flickeringLight != null)
        {
            originalIntensity = flickeringLight.intensity;
            StartCoroutine(Flicker());
        }
        else
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            float flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);

            float startTime = Time.time;
            float endTime = startTime + flickerSpeed;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / flickerSpeed;
                flickeringLight.intensity = Mathf.Lerp(originalIntensity, targetIntensity, t);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f)); // Wait before the next flicker
        }
    }
}