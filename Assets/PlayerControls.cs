using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float maxMoveSpeed = 5f; // Maximum speed when the joystick is fully pushed
    public Joystick joystick; // Reference to your Joystick
    public Transform face; // Reference to the player's face (Sprite/Transform)
    public float faceOffsetX = 0.3f; // How far the face should move towards the edge in the X-axis
    public float faceOffsetY = 0.3f; // How far the face should move towards the edge in the Y-axis
    public float faceMoveSpeed = 5f; // Speed at which the face moves towards the edge
    public float faceMovementThreshold = 0.2f; // Threshold for face movement to start

    private Rigidbody2D rb;
    private Vector2 lastDirection = Vector2.zero; // To store the last movement direction
    private string currentDirection = "idle"; // To store the current movement direction

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get the input from the joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Create a new movement vector based on joystick input, NOT normalized
        Vector2 direction = new Vector2(horizontal, vertical);

        // The player's speed should be proportional to how much the joystick is moved
        rb.linearVelocity = direction * maxMoveSpeed;  // Apply velocity based on joystick input

        // If the input magnitude is greater than the face movement threshold, move the face
        if (direction.magnitude >= faceMovementThreshold)
        {
            MoveFaceTowardsEdge(direction); // Move the face towards the edge
        }
        else
        {
            ResetFacePosition(); // Return the face to the center when input is too small
        }

        // Save the current movement direction as a string for other logic
        if (horizontal > 0)
        {
            currentDirection = "right";
        }
        else if (horizontal < 0)
        {
            currentDirection = "left";
        }
        else if (vertical > 0)
        {
            currentDirection = "up";
        }
        else if (vertical < 0)
        {
            currentDirection = "down";
        }
        else
        {
            currentDirection = "idle"; // If no input, set it to idle
        }
    }

    private void MoveFaceTowardsEdge(Vector2 direction)
    {
        // Calculate the target position for the face based on the movement direction and offsets
        Vector2 targetPosition = new Vector2(direction.x * faceOffsetX, direction.y * faceOffsetY);

        // Gradually move the face towards the target position using Lerp, based on faceMoveSpeed
        face.localPosition = Vector3.Lerp(face.localPosition, new Vector3(targetPosition.x, targetPosition.y, face.localPosition.z), faceMoveSpeed * Time.deltaTime);
    }

    private void ResetFacePosition()
    {
        // Gradually move the face back to the center position when the player is idle or moving slowly
        face.localPosition = Vector3.Lerp(face.localPosition, Vector3.zero, faceMoveSpeed * Time.deltaTime);
    }
}
