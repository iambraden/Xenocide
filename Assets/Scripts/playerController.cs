using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;

    [Header("Player movement")]
    public float moveSpeed = 5f;
    public float xMin = -4.6f; // Left boundary
    public float xMax = 4.6f;  // Right boundary

    [Header("Player sprites")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 velocity = new Vector2(horizontalInput * moveSpeed, 0);

        playerRigidbody.linearVelocity = velocity;

        // Ensure player's position stays within screen bounds
        Vector2 clampedPosition = playerRigidbody.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, xMin, xMax);
        playerRigidbody.position = clampedPosition;
        
        // Change player sprite based on movement
        if (horizontalInput < 0){ 

        playerSpriteRenderer.sprite = leftSprite;

        }else if (horizontalInput > 0){

        playerSpriteRenderer.sprite = rightSprite;

        }else{

        playerSpriteRenderer.sprite = idleSprite;

        }
        
    }
}
