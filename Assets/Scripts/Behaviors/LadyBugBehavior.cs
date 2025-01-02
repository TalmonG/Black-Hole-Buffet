using System.Collections;
using UnityEngine;

public class LadyBugBehavior : MonoBehaviour
{
    private Transform player;
    private ObjectInteractions playerInteractions;
    private Rigidbody2D rb;

    public float speed = 2f; // Movement speed
    public float changeDirectionTime = 5f; // Time to change wandering direction
    public float wanderTowardsPlayerChance = 0.5f; // Chance to wander closer to the player (0 = never, 1 = always)  KEEP MINIMUM 0.2
    public float detectionRadius = 10f;

    private Vector2 currentDirection;
    private Vector2 wanderDirection;
    private float antSize;

    public bool isBeingPulled = false; // Flag to track if the ant is being pulled

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player is tagged 'Player'.");
            return;
        }

        playerInteractions = player.GetComponent<ObjectInteractions>();

        if (playerInteractions == null)
        {
            Debug.LogError("ObjectInteractions script not found on the Player!");
            return;
        }

        if (!TryGetAntSize(out antSize))
        {
            Debug.LogError($"Ant size could not be determined for tag '{gameObject.tag}'.");
            return;
        }

        StartCoroutine(Wander());
    }

    void Update()
    {
        if (player == null || playerInteractions == null || isBeingPulled) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 targetDirection;

        if (distanceToPlayer < detectionRadius) // Detection radius
        {
            if (playerInteractions.playerSize > antSize)
            {
                // Flee from the player
                targetDirection = (transform.position - player.position).normalized;
            }
            else
            {
                // Chase the player
                targetDirection = (player.position - transform.position).normalized;
            }
        }
        else
        {
            // Continue wandering
            targetDirection = wanderDirection;
        }

        // Smoothly transition to the target direction
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * 5f);

        // Apply movement
        rb.linearVelocity = currentDirection * speed;

        RotateToFaceMovement();
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (isBeingPulled) yield break; // Stop wandering while being pulled

            // Determine whether to wander toward the player
            if (Random.value < wanderTowardsPlayerChance)
            {
                // Generate a wandering direction biased toward the player
                Vector2 toPlayer = (player.position - transform.position).normalized;
                wanderDirection = Vector2.Lerp(Random.insideUnitCircle.normalized, toPlayer, 0.5f).normalized;
            }
            else
            {
                // Generate a random wandering direction
                wanderDirection = Random.insideUnitCircle.normalized;
            }

            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    private bool TryGetAntSize(out float size)
    {
        size = 0f;
        string tag = gameObject.tag;
        float measurement;

        if (playerInteractions.GetMeasurementAndSize(tag, out measurement, out size))
        {
            return true;
        }

        return false;
    }

    private void RotateToFaceMovement()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude > 0.01f) // Only rotate if moving
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // Apply rotation offset (adjust depending on sprite's orientation)
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust -90f as needed
        }
    }
}
