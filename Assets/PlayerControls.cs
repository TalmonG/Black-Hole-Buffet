using UnityEngine;
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Joystick joystick; // Reference to your Joystick

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get the input from the joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Create a new movement vector
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        // Apply the movement to the Rigidbody2D
        rb.linearVelocity = direction * moveSpeed;
    }
}
