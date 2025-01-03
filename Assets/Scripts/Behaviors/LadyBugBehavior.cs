using System.Collections;
using UnityEngine;

public class LadybugBehavior : MonoBehaviour
{
    private Transform player;
    private ObjectInteractions playerInteractions;
    private Rigidbody2D rb;

    public float speed = 2f;
    public float changeDirectionTime = 5f;
    public float wanderTowardsPlayerChance = 0.5f;
    public float detectionRadius = 10f;

    private Vector2 currentDirection;
    private Vector2 wanderDirection;
    private float antSize;

    public bool isBeingPulled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        playerInteractions = player.GetComponent<ObjectInteractions>();

        if (!TryGetLadybugSize(out antSize))
        {
            Debug.LogError($"Ant size could not be determined for tag '{gameObject.tag}'.");
            return;
        }

        StartCoroutine(Wander());
    }

    void Update()
    {
        if (player == null || playerInteractions == null || isBeingPulled) return; // make sure things not broken so i break before it break me >:)

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 targetDirection;

        if (distanceToPlayer < detectionRadius) // detection radius
        {
            if (playerInteractions.playerSize > antSize) // flee if player bigger
            {
                targetDirection = (transform.position - player.position).normalized;
            }
            else
            {
                // chase player
                targetDirection = (player.position - transform.position).normalized;
            }
        }
        else
        {
            // keep wandering
            targetDirection = wanderDirection;
        }

        // attempt transition to the target direction, seems better after added but idk. (keep this just in case)
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * 5f);

        rb.linearVelocity = currentDirection * speed;

        RotateToFaceMovement();
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (isBeingPulled) yield break; // stop wandering if pulled

            // should wander to player?
            if (Random.value < wanderTowardsPlayerChance)
            {
                // generate wandering direction toward the player
                Vector2 toPlayer = (player.position - transform.position).normalized;
                wanderDirection = Vector2.Lerp(Random.insideUnitCircle.normalized, toPlayer, 0.5f).normalized;
            }
            else
            {
                // generate random wandering direction
                wanderDirection = Random.insideUnitCircle.normalized;
            }

            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    // get lady bug size
    private bool TryGetLadybugSize(out float size)
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

    // rotate ladybug to face movement
    private void RotateToFaceMovement()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude > 0.01f) // only rotate if moving
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}
