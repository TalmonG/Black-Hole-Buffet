using UnityEngine;
using UnityEngine.UI;

public class GravityPull : MonoBehaviour
{
    public float cooldownDuration = 5f;  // Cooldown duration in seconds
    public Image cooldownImage;         // Reference to the cooldown overlay
    private Button button;              // Reference to the button
    private bool isCooldown = false;    // Track cooldown state

    private float pullStrength = 10f;   // Strength of the pull effect
    private float pullRadius = 5f;      // Radius of the pull effect

    void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateGravityPull);

        // Ensure the cooldownImage starts empty
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
    }

    void OnDrawGizmosSelected()
    {
        // Draw the pull radius in the editor for testing purposes
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, pullRadius);
        }
    }

    void ActivateGravityPull()
    {
        if (isCooldown) return;

        Debug.Log("Gravity Pull activated!");

        // Perform the gravity pull functionality
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, pullRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Ant"))
                {
                    Vector3 direction = player.transform.position - collider.transform.position;
                    collider.attachedRigidbody?.AddForce(direction.normalized * pullStrength, ForceMode.Impulse);
                }
            }
        }

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
