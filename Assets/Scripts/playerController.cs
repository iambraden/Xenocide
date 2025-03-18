using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public float moveSpeed = 5f;
    public float xMin = -4.6f; // Left boundary
    public float xMax = 4.6f;  // Right boundary

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 velocity = new Vector2(horizontalInput * moveSpeed, 0);

        playerRigidbody.linearVelocity = velocity;

        // Ensure player's position stays within screen bounds
        Vector2 clampedPosition = playerRigidbody.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, xMin, xMax);
        playerRigidbody.position = clampedPosition;
    }
}
