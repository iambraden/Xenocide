using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet speed
    public float speed = 10f; 
    public float enemySpeed = 10f;

    //Bullet liftetime
    public float lifetime = 2f;
    public int health = 1;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Move();
    }


    void Move()
    {
        if(this.CompareTag("Bullet"))
        {
            // Move up for player bullets
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if(this.CompareTag("EnemyBullet"))
        {
            // Move down for enemy bullets
            transform.Translate(Vector2.down * enemySpeed * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet destroyed when colliding with enemy or enemy bullet
        if (this.CompareTag("Bullet"))
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                this.health--;
                if (this.health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        // Enemy bullet destroyed when colliding with player or player bullet
        else if (this.CompareTag("EnemyBullet"))
        {
            if (other.CompareTag("Player"))
            {
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("Bullet"))
            {
                this.health--;
                if (this.health <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                // Check if this bullet is of the EnemyBulletLarge prefab
                if (gameObject.name.Contains("EnemyBulletLarge"))
                {
                    // Ensure the other bullet is not already destroyed
                    if (other != null && other.gameObject != null)
                    {
                        Destroy(other.gameObject); // Destroy the other enemy bullet
                    }
                }
            }
        }
    }

    public void increaseBulletSpeed(){
        speed += speed * 0.3f;
    }
}