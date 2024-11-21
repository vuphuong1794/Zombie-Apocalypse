using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEffect : MonoBehaviour
{
    public Light2D Light; // Reference to the 2D light
    public float maxIntensity = 15f; // Maximum light intensity
    public float minIntensity = 0f;  // Minimum light intensity
    public float fadeDuration = 3f;  // Duration for fading in/out

    private void Start()
    {
        if (Light == null)
        {
            Light = GetComponent<Light2D>();
        }

        // Start the pulsing light effect
        StartCoroutine(PulseLight());
    }

    private IEnumerator PulseLight()
    {
        while (true)
        {
            // Fade out
            yield return StartCoroutine(FadeLight(minIntensity, fadeDuration));
            // Fade in
            yield return StartCoroutine(FadeLight(maxIntensity, fadeDuration));
        }
    }

    private IEnumerator FadeLight(float targetIntensity, float duration)
    {
        float startIntensity = Light.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null; // Wait for the next frame
        }

        Light.intensity = targetIntensity; // Ensure exact target intensity
    }
}
