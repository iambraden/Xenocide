using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public int damage = 1;
    
    private HealthDisplay healthDisplay; //Reference to Health Display

    void Start()
    {
        health = maxHealth;
        // Find the health display once at start
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
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
            this.health--;
            // Debug line to make sure collision is working
            // Debug.Log("Player hit by enemy bullet! Current health: " + this.health);
            
            // Destroy enemy bullet on contact
            Destroy(other.gameObject);
            
            if(this.health <= 0)
            {
                healthDisplay.health = this.health;
                healthDisplay.UpdateHeartDisplay();
                Debug.Log("Player destroyed - Game Over!");
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int amount){
        health -= amount;
        
        // Force UI to update
        if (healthDisplay != null)
        {
            healthDisplay.health = this.health;
            healthDisplay.UpdateHeartDisplay();
        }
        
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
