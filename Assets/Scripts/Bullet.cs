using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet speed
    public float speed = 10f; 
    [SerializeField] private float enemySpeed = 4f;

    //Bullet liftetime
    public float lifetime = 2f;
    public float enemyLifetime = 8f;

    void Start()
    {
        //different lifetimes for player and enemy bullets
        if(gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject, lifetime);
        }
        else if(gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject, enemyLifetime);
        }
        
    }

    void Update()
    {
        //move bullet
        Move();
    }


    void Move()
    {
        //different speeds for player and enemy bullets
        if(gameObject.CompareTag("Bullet"))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if(gameObject.CompareTag("EnemyBullet"))
        {
            enemySpeed = 4f;
            transform.Translate(Vector2.down * enemySpeed * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //player bullet destroyed when colliding with enemy or enemy bullet
        if(this.CompareTag("Bullet"))
        {
            if(other.CompareTag("Enemy"))
                Destroy(this.gameObject);
            else if(other.CompareTag("EnemyBullet"))
                Destroy(this.gameObject);
        }

        //enemy bullet destroyed when colliding with player or player bullet
        else if(this.CompareTag("EnemyBullet"))
        {
            if(other.CompareTag("Player"))
                Destroy(this.gameObject);
            else if(other.CompareTag("Bullet"))
                Destroy(this.gameObject);
        }
    }


}