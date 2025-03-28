using UnityEngine;

public class BulletWave : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 8f;
    public int health = 1;
    public float frequency = 2f;
    public float amplitude = 2f;
    private int waveDirection = 1; // 1 for right, -1 for left
    Vector2 startPos;
    Vector2 pos;
    private float timeSinceSpawn = 0f; //tracks time since bullet spawned, used for sine wave movement

    void Start()
    {
        timeSinceSpawn = 0f;    //ensures sin pattern starts at the firepoint on bullet creation
        startPos = transform.position;
        pos = startPos;
        if (gameObject.name.Contains("EnemyBulletWave"))
        {
            waveDirection = Random.value > 0.5f ? 1 : -1; // Randomly choose the direction of the bullet, does not apply to boss
        }
        Destroy(gameObject, lifetime);
    }


    void Update()
    {
        Move();
    }


    void Move()
    {
        timeSinceSpawn += Time.deltaTime;
        pos.y -= speed * Time.deltaTime;
        pos.x = startPos.x + waveDirection * Mathf.Sin(timeSinceSpawn * frequency) * amplitude;
        transform.position = pos;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //no implementation for players to have this kind of bullet
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("Bullet"))
        {
            this.health--;
            if (this.health <= 0)
            {
                Debug.Log("Bullet destroyed");
                Destroy(gameObject);
            }
        }
    }

}
