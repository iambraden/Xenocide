using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Spawn")]
    private PlayerController playerController; // Reference to PlayerController
    public Vector2 startPosition;
    private bool isMovingDown = true;
    private float moveDownSpeed = 1f;

    [Header("Movement")]
    private bool movingRight = true;
    public float horizontalSpeed = 3f;
    public float xMin = -4.0f;
    public float xMax = 4.0f;
    public float turnTimerLow = 2f;
    public float turnTimerHigh = 4f;
    private float turnTimer;
    public float turnThreshold = 1.5f;

    [Header("Health")]
    public int health = 50;

    [Header("Shooting")]
    public GameObject bulletPrefabCenter;
    public Transform firePointCenter;
    public float fireTimerCenterLow = 0.8f;
    public float fireTimerCenterHigh = 1.5f;
    private float firetimerCenter;
    public GameObject bulletPrefabSide;
    public Transform firePointLeft;
    public Transform firePointRight;

    private GameManager gameManager;

    

    void Start()
    {
        // Start the boss above the screen
        transform.position = new Vector2(transform.position.x, transform.position.y);

        //Find game manager
        gameManager = FindFirstObjectByType<GameManager>();

        //handle difficulty scaling
        HandleDifficulty();
    
        // Find the player and disable their shooting
        playerController = FindFirstObjectByType<PlayerController>();
        playerController.canShoot = false; // Disable shooting

        startPosition = new Vector2(transform.position.x, 3.5f);
        StartCoroutine(MoveBossDownCoroutine());
    }

    public void HandleDifficulty()
    {
        int difficulty = gameManager.difficulty;
        Debug.Log($"[HandleDifficulty] Difficulty: {difficulty}");
        if(difficulty == 0)
        {
            return;
        }
        
        //increase stats based on difficulty
        health = Mathf.RoundToInt(health * Mathf.Pow(1.5f, difficulty));
        fireTimerCenterLow *= Mathf.Pow(0.9f, difficulty);
        fireTimerCenterHigh *= Mathf.Pow(0.9f, difficulty);
    }

    void Update()
    {
        if(isMovingDown)
        {
            return;
        }

        Move();

        firetimerCenter -= Time.deltaTime;
            if (firetimerCenter <= 0)
            {
                firetimerCenter = Random.Range(fireTimerCenterLow, fireTimerCenterHigh);
                FireBulletCenter();
                //TODO: put a different timer on side bullets
                FireBulletSide();
            }
    }

    void Move()
    {
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * horizontalSpeed * Time.deltaTime);

        bool hitEdge = false;
        if (transform.position.x >= xMax)
        {
            hitEdge = true;
        }
        else if (transform.position.x <= xMin)
        {
            hitEdge = true;
        }

        if(hitEdge) // Reverse direction if edge hit
        {
            movingRight = !movingRight;
        } 
        else // If not at edge, check for a random turn
        {
            if(Mathf.Abs(transform.position.x - xMax) > turnThreshold && Mathf.Abs(transform.position.x - xMin) > turnThreshold)
            {
                turnTimer -= Time.deltaTime;
                if(turnTimer <= 0)
                {
                    
                    movingRight = !movingRight;
                    turnTimer = Random.Range(turnTimerLow, turnTimerHigh);
                }
            }
        }
    }


    void FireBulletCenter()
    {
        GameObject newBulletPrefab = Instantiate(bulletPrefabCenter, firePointCenter.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("EnemyBullets").transform);
        SoundManager.PlaySound(SoundType.EnemyShoot);
    }

    void FireBulletSide()
    {
        GameObject newBulletPrefab = Instantiate(bulletPrefabSide, firePointLeft.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("EnemyBullets").transform);
        
        newBulletPrefab = Instantiate(bulletPrefabSide, firePointRight.position, Quaternion.identity);
        newBulletPrefab.transform.SetParent(GameObject.Find("EnemyBullets").transform);
        SoundManager.PlaySound(SoundType.EnemyShoot);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //player bullets damage enemy, destroyed when health = 0
        if(other.CompareTag("Bullet"))
        {
            this.health--;
            if (this.health <= 0)
            {
                SoundManager.PlaySound(SoundType.EnemyDeath, 0.3f);
                gameManager.AddScore(500);
                
                gameManager.StartCoroutine(gameManager. OnBossDefeated());  // Notify the GameManager that the boss is dead
                Destroy(gameObject);
            }
            else
            {
                SoundManager.PlaySound(SoundType.EnemyHit, 0.5f);
            }
        }

    }


    IEnumerator MoveBossDownCoroutine()
    {
        while (Vector2.Distance(transform.position, startPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                startPosition,
                moveDownSpeed * Time.deltaTime
            );
            yield return null;
        }
        playerController.canShoot = true; // Enable shooting
        isMovingDown = false;
        turnTimer = Random.Range(turnTimerLow, turnTimerHigh); //Enable turning
        firetimerCenter = Random.Range(fireTimerCenterLow, fireTimerCenterHigh); //Enable firing
    }

}
