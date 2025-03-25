using UnityEngine;

public class BossBulletSide : MonoBehaviour
{
    public float speed = 3f;
    public float lifetime = 8f;
    public int health = 1;
    public float frequency = 2f;
    public float amplitude = 2f;
    Vector2 startPos;
    Vector2 pos;

    void Start()
    {
        startPos = transform.position;
        pos = startPos;
        Destroy(gameObject, lifetime);
    }


    void Update()
    {
        Move();
    }


    void Move()
    {
        pos.y -= speed * Time.deltaTime;
        pos.x = startPos.x + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = pos;
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
