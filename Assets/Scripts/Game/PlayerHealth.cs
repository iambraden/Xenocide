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

    void Start(){
        health = maxHealth;
        // Find the health display once at start
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        gameManager = FindFirstObjectByType<GameManager>();
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

        // Force UI to update
        if (healthDisplay != null){
            healthDisplay.health = this.health;
            healthDisplay.UpdateHeartDisplay();
        }

        if (health <= 0){
            Debug.Log("PLAYER DEAD");
            // Game Over Screen
            if (gameManager != null){
                gameManager.OnPlayerDeath();
            }

            Debug.Log("Player destroyed - Game Over!");
            Destroy(gameObject);
        }
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
