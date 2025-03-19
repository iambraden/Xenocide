using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 startPos;

    public float maxDistance = 5f;
    public float frequency = 2f;
    public float phaseOffset;

    public int health = 3;
    public GameObject bulletPrefab;
     public Transform firePoint;
     private float firetimer;
     private float fireTimerLow;
     private float fireTimerHigh;

    void Start()
    {
        fireTimerLow = 2f;
        fireTimerHigh = 4f;
        firetimer = Random.Range(fireTimerLow, fireTimerHigh);
        startPos = transform.position;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI); // Random phase offset for variation
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        firetimer -= Time.deltaTime;
        if(firetimer <= 0)
        {
            firetimer = Random.Range(fireTimerLow, fireTimerHigh);
            FireBullet();
        }
    }


    void Move()
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


    void FireBullet()
    {
        GameObject newBulletPrefab = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("EnemyBullets").transform);
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