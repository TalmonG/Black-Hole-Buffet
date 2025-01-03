using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float maxMoveSpeed = 5f; 
    public Joystick joystick; 
    public Transform face; 
    public float faceOffsetX = 0.3f; 
    public float faceOffsetY = 0.3f; 
    public float faceMoveSpeed = 5f;
    public float faceMovementThreshold = 0.2f; 

    private Rigidbody2D rb;
    private Vector2 lastDirection = Vector2.zero; 
    private string currentDirection = "idle"; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get input from joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Create new movement vector based on joystick input, NOT normalized
        Vector2 direction = new Vector2(horizontal, vertical);

        // players speed proportional to how much joystick moved
        rb.linearVelocity = direction * maxMoveSpeed;  

        //move the face
        if (direction.magnitude >= faceMovementThreshold)
        {
            MoveFaceTowardsEdge(direction);
        }
        else
        {
            ResetFacePosition();
        }

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
            currentDirection = "idle";
        }
    }

    private void MoveFaceTowardsEdge(Vector2 direction)
    {
        // Calculate target position for face based on movement direction and offsets
        Vector2 targetPosition = new Vector2(direction.x * faceOffsetX, direction.y * faceOffsetY);

        // Gradually move face towards the target position
        face.localPosition = Vector3.Lerp(face.localPosition, new Vector3(targetPosition.x, targetPosition.y, face.localPosition.z), faceMoveSpeed * Time.deltaTime);
    }

    private void ResetFacePosition()
    {
        // Gradually move face back to center position when the player is idle or moving slowly
        face.localPosition = Vector3.Lerp(face.localPosition, Vector3.zero, faceMoveSpeed * Time.deltaTime);
    }
}
