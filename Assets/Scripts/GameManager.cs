using UnityEngine;

public class GameManager : MonoBehaviour
{

     [Header("Wave Settings")]
    public float waveInterval = 22f;
    private float waveTimer;
    public FormationManager formationPrefab; // Reference to formation prefab

    public GameObject enemy; // Reference to the wave enemy prefab
    public GameObject boss; // Reference to the boss prefab
    public float enemySpawnInterval = 4f;
    private float enemySpawnTimer;
    private float duoWaveSpawnChance = 25f;
    private bool isBossActive = false; // Flag to track if the boss is active
        void Start(){
        SoundManager.PlaySound(SoundType.GameMusic, 0.5f);
        waveTimer = waveInterval; // Start first wave after full interval
    }

    void Update(){
        // Skip enemy spawning if the boss is active
        if (isBossActive) return;

        // Enemy spawning
        HandleRandomEnemySpawns();
        
        // Wave timing
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            SpawnFormationWave();
            waveTimer = waveInterval;
        }

        // Forced boss spawn for now
        if (Input.GetKeyDown(KeyCode.B)) SpawnBoss();
    }

    void SpawnFormationWave(){
        // Create new formation instance
        FormationManager newFormation = Instantiate(formationPrefab, new Vector3(0, 7f, 0), Quaternion.identity);
        newFormation.transform.SetParent(GameObject.Find("Enemies").transform);
        newFormation.StartNewWave();
    }

    void HandleRandomEnemySpawns(){
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval){
            // 25% chance to spawn two enemies side-by-side
            if (Random.Range(0, 100) < duoWaveSpawnChance){
                spawnDuoEnemy();
                enemySpawnTimer = 0f; // Reset timer on enemy spawn
                return;
            }
            SpawnEnemy();
            enemySpawnTimer = 0f; // Reset timer on enemy spawn
        }
    }

    void SpawnEnemy(){
        //spawn enemies at a random x-position just above the screen
        float randomX = Random.Range(-4f, 4f);
        Vector2 spawnPosition = new Vector2(randomX, 7f);
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(GameObject.Find("Enemies").transform);

        // Randomly assign enemy type
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null){
            enemyController.enemyType = (EnemyController.EnemyType)Random.Range(0, 2);
            //TODO: implement sway enemy type
                //only make wave enemies for now
            enemyController.enemyType = EnemyController.EnemyType.Wave;
        }
    }

    void spawnDuoEnemy(){
        float spacing = 1f;
        float randomX = Random.Range(-4f, 4f);
        Vector2 spawnPosition = new Vector2(randomX, 7f);

        // Instantiate the first enemy
        GameObject newEnemy1 = Instantiate(enemy, spawnPosition, Quaternion.identity);
        newEnemy1.transform.SetParent(GameObject.Find("Enemies").transform);

        // Instantiate the second enemy with spacing
        GameObject newEnemy2 = Instantiate(enemy, spawnPosition + new Vector2(spacing, 0), Quaternion.identity);
        newEnemy2.transform.SetParent(GameObject.Find("Enemies").transform);

        // Assign the same startPos and phaseOffset to both enemies
        EnemyController enemyController1 = newEnemy1.GetComponent<EnemyController>();
        EnemyController enemyController2 = newEnemy2.GetComponent<EnemyController>();

        if (enemyController1 != null && enemyController2 != null){
            // Set both enemies to Wave type
            enemyController1.enemyType = EnemyController.EnemyType.Wave;
            enemyController2.enemyType = EnemyController.EnemyType.Wave;

            // Synchronize their movement by sharing startPos and phaseOffset
            Vector2 sharedStartPos = spawnPosition;
            float sharedPhaseOffset = Random.Range(0f, 2f * Mathf.PI);

            enemyController1.startPos = sharedStartPos;
            enemyController1.phaseOffset = sharedPhaseOffset;

            enemyController2.startPos = sharedStartPos;
            enemyController2.phaseOffset = sharedPhaseOffset;
        }

        Debug.Log("Spawned duo enemies moving in sync");
    }

    public void SpawnBoss(){
        // Stop enemy spawning
        isBossActive = true;

        // Spawn the boss at the top of the screen
        Vector2 bossSpawnPosition = new Vector2(0, 7f); // Centered at the top
        GameObject newBoss = Instantiate(boss, bossSpawnPosition, Quaternion.identity);
        newBoss.transform.SetParent(GameObject.Find("Enemies").transform);

        Debug.Log("Boss spawned. Enemy spawning stopped.");
    }


}