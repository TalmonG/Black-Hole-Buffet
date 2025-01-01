using UnityEngine;
using UnityEngine.UI;

public class GravityPull : MonoBehaviour
{
    public float cooldownDuration = 5f;  // Cooldown duration in seconds
    public Image cooldownImage;         // Reference to the cooldown overlay
    private Button button;              // Reference to the button
    private bool isCooldown = false;    // Track cooldown state

    public float pullStrength = 10f;   // Strength of the pull effect
    public float basePullRadius = 30f;  // Base radius to be multiplied by player size

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
            float pullRadius = CalculatePullRadius(player.transform);
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
            float pullRadius = CalculatePullRadius(player.transform);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, pullRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Ant"))
                {
                    // Disable ant movement by disabling AntBehavior script
                    AntBehavior antBehavior = collider.GetComponent<AntBehavior>();
                    if (antBehavior != null)
                    {
                        antBehavior.enabled = false;
                    }

                    // Pull ant toward player
                    Rigidbody2D rb = collider.attachedRigidbody;
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero; // Stop current motion
                        Vector2 direction = (Vector2)(player.transform.position - collider.transform.position);
                        rb.AddForce(direction.normalized * pullStrength, ForceMode2D.Impulse);
                        Debug.Log($"Pulling Ant: {collider.gameObject.name}");

                        // Re-enable AntBehavior script after pulling
                        StartCoroutine(ReenableAntBehavior(antBehavior, 1f));
                    }
                }
            }
        }

        // Start the cooldown process
        StartCooldown();
    }

    float CalculatePullRadius(Transform playerTransform)
    {
        // Calculate pull radius based on player size
        return basePullRadius * playerTransform.localScale.x;
    }

    System.Collections.IEnumerator ReenableAntBehavior(AntBehavior antBehavior, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (antBehavior != null)
        {
            antBehavior.enabled = true;
        }
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
