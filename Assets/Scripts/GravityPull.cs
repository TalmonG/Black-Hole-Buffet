using UnityEngine;
using UnityEngine.UI;

public class GravityPull : MonoBehaviour
{
    public float cooldownDuration = 5f;
    public Image cooldownImage;
    public GameObject tutorialGameObject;
    private Button button;
    private bool isCooldown = false;
    private bool tutorialActive = true;

    public float pullStrength = 10f;
    public float basePullRadius = 30f;

    void Start()
    {
        // Get Button component on GameObject
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateGravityPull);

        // Set cooldownImage empty
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;

        // Enable tutorial GameObject at start
        if (tutorialGameObject != null)
        {
            tutorialGameObject.SetActive(true);
        }
    }

    // DONT NEED THIS BELOW
    void OnDrawGizmosSelected()
    {
        // Draw radius for testing purpose
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

        // do gravity pull
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

        // Start cooldown
        StartCooldown();
    }

    void DisableTutorial()
    {
        if (tutorialGameObject != null)
        {
            tutorialGameObject.SetActive(false);
        }
        tutorialActive = false; 
    }

    void PullObject(Collider2D collider, System.Type behaviorType)
    {
        // Disable insect behavior script when pulling
        MonoBehaviour behavior = (MonoBehaviour)collider.GetComponent(behaviorType);
        if (behavior != null)
        {
            behavior.enabled = false;
        }

        // Pull insect toward player
        Rigidbody2D rb = collider.attachedRigidbody;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (rb != null && player != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop
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
        button.interactable = false; // Disable button during cooldown

        if (cooldownImage != null)
            cooldownImage.fillAmount = 1; // Start cooldown visual

        StartCoroutine(CooldownRoutine());
    }

    System.Collections.IEnumerator CooldownRoutine()
    {
        float timeElapsed = 0;

        // Gradually reduce cooldown image's fill amount
        while (timeElapsed < cooldownDuration)
        {
            timeElapsed += Time.deltaTime;
            if (cooldownImage != null)
                cooldownImage.fillAmount = 1 - (timeElapsed / cooldownDuration);
            yield return null;
        }

        // Reset cooldown
        isCooldown = false;
        button.interactable = true; // Re-enable button
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0; // Reset visual
    }
}
