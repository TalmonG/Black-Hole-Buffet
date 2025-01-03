using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    // start fade-out process
    public void FadeOutBackground(GameObject level)
    {
        StartCoroutine(FadeOutMaterial(level));
    }

    private IEnumerator FadeOutMaterial(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        Material material = renderer.material;

        Color color = material.color; 
        float duration = 1f;         
        float elapsed = 0;

        color.a = 1;
        material.color = color;

        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(1, 0, elapsed / duration);
            material.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = 0; 
        material.color = color;

        obj.SetActive(false);
    }
}
