using System.Collections;
using UnityEngine;

public class AntBehavior : MonoBehaviour
{
    private Transform player;
    private ObjectInteractions playerInteractions;
    private Rigidbody2D rb;

    public float speed = 2f; // Movement speed
    public float changeDirectionTime = 5f; // Time to change wandering direction
    private Vector2 currentDirection;
    private Vector2 wanderDirection;
    private float antSize;

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
        if (player == null || playerInteractions == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 targetDirection;

        if (distanceToPlayer < 5f) // Detection radius
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
            // Generate a new random wandering direction
            wanderDirection = Random.insideUnitCircle.normalized;
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
        // Get the current velocity direction
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude > 0.01f) // Only rotate if moving
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // Apply rotation offset (adjust depending on sprite's orientation)
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust -90f as needed
        }
    }
}
