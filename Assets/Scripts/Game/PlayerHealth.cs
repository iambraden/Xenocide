using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public int damage = 1;
    private HealthDisplay healthDisplay; //Reference to Health Display
    private GameManager gameManager;
    private bool isInvincible = false;
    
    // Flash effect variables
    private SpriteRenderer playerSprite;
    private bool isFlashing = false;
    [Header("Damage Visual")]
    public float flashDuration = 0.1f;
    public int flashCount = 3;
    public Color flashColor = Color.red;

    [Header("Death Effects")]
    [SerializeField] private GameObject playerExplosionPrefab;
    [SerializeField] private float explosionAnimationDuration = 1.5f;

    void Start(){
        health = maxHealth;
        // Find the health display once at start
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        gameManager = FindFirstObjectByType<GameManager>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (health > maxHealth){
            health = maxHealth;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (isInvincible) return; // Ignore collisions if invincible

        // bullets damage player, game lost when health = 0
        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy")){
            SoundManager.PlaySound(SoundType.PlayerHit, 3f);

            // Call TakeDamage (allows for variable damage)
            TakeDamage(1);

            // Destroy enemy bullet on contact
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount){
        PlayerController playerController = GetComponent<PlayerController>();
        if(playerController != null && playerController.IsForceFieldActive()){
            playerController.DeactivateForceField();
            return;
        }
        
        health -= amount;
        
        // Activate screen shake
        CameraShake.Shake();

        // Play damage flash effect
        if (!isFlashing && playerSprite != null) {
            StartCoroutine(FlashEffect());
        }

        // Force UI to update
        if (healthDisplay != null){
            healthDisplay.health = this.health;
            healthDisplay.UpdateHeartDisplay();
        }

        if (health <= 0){
            // start death sequence
            StartCoroutine(PlayerDeathSequence());
        }
    }

    private IEnumerator PlayerDeathSequence()
    {
        // disable the player's controls
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
            if (playerController.playerRigidbody != null)
            {
                playerController.playerRigidbody.linearVelocity = Vector2.zero;
            }
        }
        
        // freeze the game
        Time.timeScale = 0;

        // hide player sprite
        if (playerSprite != null)
        {
            playerSprite.enabled = false;
        }
        
        if (playerExplosionPrefab != null)
        {
            // create explosion animation at player's last position
            GameObject explosionObj = Instantiate(playerExplosionPrefab, transform.position, Quaternion.identity);
            explosionObj.SetActive(true);
            
            // grab animator
            Animator animator = explosionObj.GetComponent<Animator>();
            if (animator != null)
            {
                // animate in unscaled time so the animation plays during game freeze
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                
                // play explosion sound (set it to enemydeath for now since there's no player sound)
                SoundManager.PlaySound(SoundType.EnemyDeath, 1.0f);
                
                // wait for animation to be done
                yield return new WaitForSecondsRealtime(explosionAnimationDuration);
                
                // destroy explosion gameobject
                Destroy(explosionObj);
            }
        }
        
        // reset time scale to 1 (avoids scene change bugs)
        Time.timeScale = 1;

        // show game over screen
        if (gameManager != null)
        {
            gameManager.OnPlayerDeath();
        }
        
        // destroy the player object
        Destroy(gameObject);
    }

    private IEnumerator FlashEffect() {
        isFlashing = true;
        Color originalColor = playerSprite.color;
        
        for (int i = 0; i < flashCount; i++) {
            // Change to flash color
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            
            // Change back to original
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
        
        isFlashing = false;
    }

    public void HealPlayer(){
        if (maxHealth < 5){
            maxHealth++; // Increase max health first
        }
        health = maxHealth; // Fill to new max
        if (healthDisplay != null){
            healthDisplay.health = this.health;
            healthDisplay.maxHealth = this.maxHealth;
            healthDisplay.UpdateHeartDisplay();
        }
    }
    
    public void StartDashInvincibility(){
        isInvincible = true;
    }

    public void EndDashInvincibility(){
        isInvincible = false;
    }
}
