using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager gameManager;

    public enum EnemyType { Wave, Sway };
    public EnemyType enemyType;
    public int enemyVariant;

    [Header("Movement")]
    public float speed = 2f;
    public Vector2 startPos;
    public float maxDistance = 5f;
    public float frequency = 1f;
    public float phaseOffset;

    [Header("Health")]
    public int health = 3;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public bool shoots;
    public float fireTimerLow;
    public float fireTimerHigh;
    private float firetimer;
    private float minFireHeight = -3.0f;

    [Header("Effects")]
    public GameObject explosionPrefab;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        firetimer = Random.Range(fireTimerLow, fireTimerHigh);
        startPos = transform.position;
        if (Mathf.Abs(phaseOffset) < 0.0001f)
        {
            phaseOffset = Random.Range(0f, 2f * Mathf.PI); // Random phase offset for variation
        }
    }

    void Update()
    {
        // Move the enemy based on its type
        if (enemyType == EnemyType.Wave)
            WaveMove();

        if(shoots)
        {
            // fire bullets on a varied timer if the enemy is high enough in the screen
            firetimer -= Time.deltaTime;
            if (firetimer <= 0 && transform.position.y > minFireHeight)
            {
                firetimer = Random.Range(fireTimerLow, fireTimerHigh);
                FireBullet();
            }
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

    void FireBullet()
    {
        GameObject newBulletPrefab = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("EnemyBullets").transform);
        SoundManager.PlaySound(SoundType.EnemyShoot);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //bullets damage enemy, destroyed when health = 0
        if(other.CompareTag("Bullet"))
        {
            this.health--;
            if(this.health <= 0)    //play death sound, play death particle, increment score
            {
                GameObject particle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                particle.transform.SetParent(GameObject.Find("Particles").transform);
                Destroy(particle, 1);
                SoundManager.PlaySound(SoundType.EnemyDeath, 0.5f);
                Destroy(gameObject);
                gameManager.AddScore(100);
            }
            else
            {
                SoundManager.PlaySound(SoundType.EnemyHit, 0.5f);
            }
        }

        //enemy collides with player, destroy enemy and player takes damage
        if(other.CompareTag("Player"))
        {
            GameObject particle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(particle, 1);
            SoundManager.PlaySound(SoundType.EnemyDeath, 0.5f);
            Destroy(gameObject);
            gameManager.AddScore(100);
            //TODO: Implement player health loss
        }
    }

}