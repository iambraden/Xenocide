using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public int damage = 1;
    
    private HealthDisplay healthDisplay; //Reference to Health Display
    private GameManager gameManager;

    [Header("Healing")]
    public KeyCode healKey = KeyCode.H;
    public float healCooldown = 2f;
    private bool canHeal = true;

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

        HandleHealingInput();
    }

    void OnTriggerEnter2D(Collider2D other){
        //bullets damage player, game lost when health = 0
        if(other.CompareTag("EnemyBullet") || other.CompareTag("Enemy")){
            SoundManager.PlaySound(SoundType.PlayerHit, 2f);
            
            // Call TakeDamage (allows for variable damage)
            TakeDamage(1);
            
            // Destroy enemy bullet on contact
            Destroy(other.gameObject);
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
            Debug.Log("PLAYER DEAD");
            // Game Over Screen
            if(gameManager != null){
                gameManager.OnPlayerDeath();
            }
            
            Debug.Log("Player destroyed - Game Over!");
            Destroy(gameObject);
        }
    }

    void HandleHealingInput(){
        if(Input.GetKeyDown(healKey) && canHeal){
            HealPlayer();
            StartCoroutine(HealCooldown());
        }
    }

    public void HealPlayer(){
        if(maxHealth < 5){
            maxHealth++; // Increase max health first
           
        }
         health = maxHealth; // Fill to new max
        if(healthDisplay != null){
            healthDisplay.health = this.health;
            healthDisplay.maxHealth = this.maxHealth;
            healthDisplay.UpdateHeartDisplay();
        }
    }

    IEnumerator HealCooldown(){
        canHeal = false;
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }
}
