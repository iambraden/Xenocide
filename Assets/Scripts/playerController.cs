using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletCooldown = 0.2f;
    private float nextFireTime = 0f;
    public bool  canShoot = true;

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

    void Update(){
        if(canShoot && Input.GetKey(KeyCode.Space) && Time.time> nextFireTime){
            FireBullet();
            nextFireTime = Time.time + bulletCooldown;
        }
    }

    void FireBullet(){
        GameObject newBulletPrefab = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("Bullets").transform);
    }
}
