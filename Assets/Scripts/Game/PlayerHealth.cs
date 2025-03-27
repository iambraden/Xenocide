using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public int damage = 1;
    
    private HealthDisplay healthDisplay; //Reference to Health Display
    private GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        // Find the health display once at start
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth){
            health = maxHealth;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //bullets damage player, game lost when health = 0
        if(other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            SoundManager.PlaySound(SoundType.PlayerHit, 2f);
            
            // Call TakeDamage (allows for variable damage)
            TakeDamage(1);
            
            // Destroy enemy bullet on contact
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount){
        Debug.Log("PLAYER TOOK DAMAGE");
        health -= amount;
        
        // Force UI to update
        if (healthDisplay != null)
        {
            healthDisplay.health = this.health;
            healthDisplay.UpdateHeartDisplay();
        }
        
        if(health <= 0){
            Debug.Log("PLAYER DEAD");
            // Game Over Screen
            if(gameManager != null) {
                gameManager.OnPlayerDeath();
            }
            
            Debug.Log("Player destroyed - Game Over!");
            Destroy(gameObject);
        }
    }
}
