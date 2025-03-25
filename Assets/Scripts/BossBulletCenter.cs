using UnityEngine;

public class BossBulletCenter : MonoBehaviour
{

    public float speed = 6f;
    public float lifetime = 4f;
    public int health = 2;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("Bullet"))
        {
            this.health--;
            if (this.health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
