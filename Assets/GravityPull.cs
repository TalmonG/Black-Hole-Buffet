using UnityEngine;
using UnityEngine.UI;

public class GravityPull : MonoBehaviour
{
    public float cooldownDuration = 5f;  // Cooldown duration in seconds
    public Image cooldownImage;         // Reference to the cooldown overlay
    public GameObject tutorialGameObject; // Reference to the tutorial GameObject
    private Button button;              // Reference to the button
    private bool isCooldown = false;    // Track cooldown state
    private bool tutorialActive = true; // Track if the tutorial is active

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

        // Enable the tutorial GameObject at the start
        if (tutorialGameObject != null)
        {
            tutorialGameObject.SetActive(true);
        }
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
        if (tutorialActive)
        {
            DisableTutorial();
        }

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
                    PullObject(collider, typeof(AntBehavior));
                }
                else if (collider.CompareTag("Ladybug"))
                {
                    PullObject(collider, typeof(LadybugBehavior));
                }
            }
        }

        // Start the cooldown process
        StartCooldown();
    }

    void DisableTutorial()
    {
        if (tutorialGameObject != null)
        {
            tutorialGameObject.SetActive(false);
        }
        tutorialActive = false; // Ensure it doesn't show again
    }

    void PullObject(Collider2D collider, System.Type behaviorType)
    {
        // Disable behavior script
        MonoBehaviour behavior = (MonoBehaviour)collider.GetComponent(behaviorType);
        if (behavior != null)
        {
            behavior.enabled = false;
        }

        // Pull object toward player
        Rigidbody2D rb = collider.attachedRigidbody;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (rb != null && player != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop current motion
            Vector2 direction = (Vector2)(player.transform.position - collider.transform.position);
            rb.AddForce(direction.normalized * pullStrength, ForceMode2D.Impulse);
            Debug.Log($"Pulling {collider.gameObject.tag}: {collider.gameObject.name}");

            // Re-enable behavior script after pulling
            StartCoroutine(ReenableBehavior(behavior, 1f));
        }
    }

    float CalculatePullRadius(Transform playerTransform)
    {
        // Calculate pull radius based on player size
        return basePullRadius * playerTransform.localScale.x;
    }

    System.Collections.IEnumerator ReenableBehavior(MonoBehaviour behavior, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (behavior != null)
        {
            behavior.enabled = true;
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
