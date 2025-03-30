using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform[] firePointAr;
    public float bulletCooldown = 0.2f;
    private float nextFireTime = 0f;
    public bool canShoot = true;
    public bool twinshot = false;
    private int twinShotIndex = 0;

    [Header("Player movement")]
    public float moveSpeed = 5f;
    public float xMin = -4.6f; // Left boundary
    public float xMax = 4.6f;  // Right boundary

    [Header("Player sprites")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    [Header("Dashing")]
    public float dashSpeedMultiplier = 3f;
    public float dashDuration = 0.2f;      
    public float dashCooldown = 1f;       
    private bool canDash = false;
    private float originalSpeed;  

    void Start(){
        originalSpeed = moveSpeed;
    }

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

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Debug.Log("Dash " + canDash);
            dash();
        }
    }

    void FireBullet(){
        if (twinshot){
        // Alternate between fire points in firePointAr
        Transform selectedFirePoint = firePointAr[twinShotIndex];
        twinShotIndex = (twinShotIndex + 1) % firePointAr.Length;

        GameObject newBulletPrefab = Instantiate(bulletPrefab, selectedFirePoint.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("Bullets").transform);
        SoundManager.PlaySound(SoundType.PlayerShoot, 0.5f);

        // Adjust cooldown for twin-shot
        nextFireTime = Time.time + bulletCooldown / 2f; // Faster cooldown for twin-shot
        }else{
        // Default single-shot behavior
        GameObject newBulletPrefab = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("Bullets").transform);
        SoundManager.PlaySound(SoundType.PlayerShoot, 0.5f);

        // Default cooldown
        nextFireTime = Time.time + bulletCooldown;
        }
    }

    void dash(){
        if(canDash){
            float horizontalInput = Input.GetAxis("Horizontal");
            float dashDirection = Mathf.Sign(horizontalInput);

            if(horizontalInput == 0) return;

            StartCoroutine(DashRoutine(dashDirection));
        }
        
    }

    IEnumerator DashRoutine(float dashDirection){
        canDash = false;
        canShoot = false;

        GetComponent<PlayerHealth>().StartDashInvincibility();

        moveSpeed = originalSpeed * dashSpeedMultiplier;
        SoundManager.PlaySound(SoundType.Dash, 0.5f);
        yield return new WaitForSeconds(dashDuration);

        GetComponent<PlayerHealth>().EndDashInvincibility();

        moveSpeed = originalSpeed;
        canShoot = true;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }



    public void setCanDash(){
        canDash = true;
    }

    public void IncreaseMoveSpeed(){
        moveSpeed += moveSpeed * 0.2f;
    }

    public void IncreaseFireRate(){
        bulletCooldown -= bulletCooldown * 0.2f;
    }

    public void setTwinShot(){
        twinshot = true;
    }
    public void IncreaseBulletSpeed()
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        if (bullet != null){
            bullet.speed += bullet.speed * 0.2f;
            Debug.Log("Bullet speed increased to: " + bullet.speed);
        }
        else{
            Debug.LogError("Bullet prefab does not have a Bullet component!");
        }
    }
}
