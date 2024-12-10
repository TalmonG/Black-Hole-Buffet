using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    // Method to start the fade-out process
    public void FadeOutBackground(GameObject level)
    {
        StartCoroutine(FadeOutMaterial(level));
    }

    private IEnumerator FadeOutMaterial(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("GameObject does not have a Renderer component.");
            yield break;
        }

        Material material = renderer.material;

        // Ensure the material has a _Color property to control alpha
        if (!material.HasProperty("_Color"))
        {
            Debug.LogError("Material does not have a _Color property.");
            yield break;
        }

        Color color = material.color; // Get the current color
        float duration = 1f;          // Fade duration in seconds
        float elapsed = 0;

        color.a = 1;                  // Start fully opaque
        material.color = color;

        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(1, 0, elapsed / duration); // Interpolate alpha
            material.color = color; // Apply the new alpha
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        color.a = 0; // Ensure the final alpha is fully transparent
        material.color = color;

        obj.SetActive(false); // Optional: Disable the GameObject after fading out
    }
}
