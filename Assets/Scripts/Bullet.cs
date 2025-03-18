using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; 
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the bullet when it hits something
        Destroy(gameObject);
    }
}