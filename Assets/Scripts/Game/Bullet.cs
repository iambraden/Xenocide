using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet speed
    public float speed = 10f; 

    //Bullet liftetime
    public float lifetime = 2f;

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
            // Move downwards for enemy bullets
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if(this.CompareTag("EnemyBullet"))
        {
            // Move upwards for player bullets
            transform.Translate(Vector2.down * speed * Time.deltaTime);
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
                Destroy(this.gameObject);
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
                Destroy(this.gameObject);
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
}