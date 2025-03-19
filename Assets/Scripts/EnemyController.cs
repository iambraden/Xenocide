using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 startPos;

    public float maxDistance = 5f;
    public float frequency = 2f;
    public float phaseOffset;

    public int health = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI); // Random phase offset for variation
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        if (pos.y < -5.5f)
        {
            pos.y = 5.5f;
        }

        pos.y = pos.y - Time.deltaTime * speed;
        // Calculate sine wave motion around the initial x-position
        pos.x = startPos.x + Mathf.Sin((pos.y * frequency) + phaseOffset) * 2;
        transform.position = pos;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //bullets damage enemy, destroyed when health = 0
        if(other.CompareTag("Bullet"))
        {
            this.health--;
            if(this.health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


}