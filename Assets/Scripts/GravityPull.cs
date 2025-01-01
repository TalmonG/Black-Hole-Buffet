using UnityEngine;
using UnityEngine.UI;

public class GravityPull : MonoBehaviour
{
    public float cooldownDuration = 5f;  // Cooldown duration in seconds
    public Image cooldownImage;         // Reference to the cooldown overlay
    private Button button;              // Reference to the button
    private bool isCooldown = false;    // Track cooldown state

    void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateGravityPull);

        // Ensure the cooldownImage starts empty
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
    }

    void ActivateGravityPull()
    {
        if (isCooldown) return;

        // Perform the gravity pull functionality
        Debug.Log("Gravity Pull activated!");

        // Add your gravity pull logic here (e.g., pulling objects toward the player)

        // Start the cooldown process
        StartCooldown();
    }

    void StartCooldown()
    {
        isCooldown = true;
        button.interactable = false; // Disable the button during cooldown

        if (cooldownImage != null)
            cooldownImage.fillAmount = 1; // Start the cooldown visual

        StartCoroutine(CooldownRoutine());
    }

    System.Collections.IEnumerator CooldownRoutine()
    {
        float timeElapsed = 0;

        // Gradually reduce the cooldown image's fill amount
        while (timeElapsed < cooldownDuration)
        {
            timeElapsed += Time.deltaTime;
            if (cooldownImage != null)
                cooldownImage.fillAmount = 1 - (timeElapsed / cooldownDuration);
            yield return null;
        }

        // Reset cooldown
        isCooldown = false;
        button.interactable = true; // Re-enable the button
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0; // Reset visual
    }
}
