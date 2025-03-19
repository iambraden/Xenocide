using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement
    public float speed = 2f;
    private Vector2 startPos;
    public float maxDistance = 5f;
    public float frequency = 2f;
    public float phaseOffset;

    // Enemy Type
    public enum EnemyType { Wave, Sway }
    public EnemyType enemyType;

    // Health
    public int health = 3;

    // Shooting
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float fireTimerLow = 2f;
    private float fireTimerHigh = 4f;
    private float firetimer;
    private float minFireHeight = -3.0f;


    void Start()
    {
        firetimer = Random.Range(fireTimerLow, fireTimerHigh);
        startPos = transform.position;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI); // Random phase offset for variation
    }

    void Update()
    {
        // Move the enemy based on its type
        if (enemyType == EnemyType.Wave)
            WaveMove();
        else if (enemyType == EnemyType.Sway)
            SwayMove();

        //fire bullets on a varied timer if the enemy is high enough in the screen
        firetimer -= Time.deltaTime;
        if(firetimer <= 0 && transform.position.y > minFireHeight)
        {
            firetimer = Random.Range(fireTimerLow, fireTimerHigh);
            FireBullet();
        }
    }


    void WaveMove()
    {
        Vector2 pos = transform.position;

        //wrap enemy back up to top of the screen
        if (pos.y < -5.5f)
        {
            pos.y = 5.5f;
        }

        pos.y = pos.y - Time.deltaTime * speed;
        // Calculate sine wave motion around the initial x-position
        pos.x = startPos.x + Mathf.Sin((pos.y * frequency) + phaseOffset) * 2;
        transform.position = pos;
    }


    void SwayMove()
    {
        /*TODO: Implement Movement for the enemies that sit 
        near the top of the screen and move in the grid pattern*/
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