using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Wave Settings")]
    public float waveInterval = 22f;
    private float waveTimer;
    public FormationManager formationPrefab; // Reference to formation prefab

    [Header("Enemy Spawning")]
    public GameObject enemy; // Reference to the wave enemy prefab
    private bool canSpawnEnemies = true; // Controls whether enemies can spawn
    public float enemySpawnInterval = 4f;
    private float enemySpawnTimer;
    private float duoWaveSpawnChance = 25f;
    private float bossActiveSpawnAdjustment = 2f; // Decreases enemy spawn rate when boss is active

    [Header("Boss")]
    public GameObject boss; // Reference to the boss prefab
    private bool isBossActive = false; // Flag to track if the boss is active
    private float bossSpawnCooldown = 5f; // Time to wait before resuming enemy spawning

    [Header("Game Over")]
    public GameOverScreen gameOverScreen;
    public int currentScore = 1020; // track score, set to 1020 for now to test

    void Start(){
        SoundManager.PlaySound(SoundType.GameMusic, 0.5f);
        waveTimer = waveInterval; // Start first wave after full interval
    }

    void Update(){
        // Enemy spawning is delayed for 5 seconds after the boss spawns
        if (isBossActive && !canSpawnEnemies)
        {
            bossSpawnCooldown -= Time.deltaTime;
            if (bossSpawnCooldown <= 0f)
            {
                canSpawnEnemies = true; // Allow enemies to spawn again
                bossSpawnCooldown = 5f; // Reset cooldown for future boss spawns
            }
        }

        // Handle enemy spawning if allowed
        if (canSpawnEnemies)
        {
            HandleRandomEnemySpawns();
        }
        
        // Wave timing
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            SpawnFormationWave();
            float adjustedWaveInterval = isBossActive ? waveInterval * bossActiveSpawnAdjustment : waveInterval;
            waveTimer = adjustedWaveInterval; // Reset wave timer
        }

        //TODO: Forced boss spawn for now
        if (Input.GetKeyDown(KeyCode.B)) SpawnBoss();
    }

    void SpawnFormationWave(){
        // Create new formation instance
        FormationManager newFormation = Instantiate(formationPrefab, new Vector2(0, 7f), Quaternion.identity);
        newFormation.transform.SetParent(GameObject.Find("Enemies").transform);
        newFormation.StartNewWave();
    }

    void HandleRandomEnemySpawns(){
        enemySpawnTimer += Time.deltaTime;

        // Halve the spawn rate if the boss is active
        float adjustedSpawnInterval = isBossActive ? enemySpawnInterval * bossActiveSpawnAdjustment : enemySpawnInterval;

        if (enemySpawnTimer >= adjustedSpawnInterval){
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

        //spawn Wave enemy
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null){
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
        // Stop enemy spawning temporarily
        isBossActive = true;
        canSpawnEnemies = false; // Disable enemy spawning for 5 seconds

        // Stop normal game music and start boss music
        SoundManager.StopMusic();
        SoundManager.PlaySound(SoundType.BossMusic);

        // Spawn the boss at the top of the screen
        Vector2 bossSpawnPosition = new Vector2(0, 7f); // Centered at the top
        GameObject newBoss = Instantiate(boss, bossSpawnPosition, Quaternion.identity);
        newBoss.transform.SetParent(GameObject.Find("Enemies").transform);

        Debug.Log("Boss spawned. Enemy spawning halved and delayed.");
    }

    public void OnBossDefeated()
    {
        isBossActive = false; // Reset the boss active flag
        Debug.Log("Boss defeated. Enemy spawning resumed at normal rate.");
    }

    public void OnPlayerDeath()
    {
        // Stop gameplay
        canSpawnEnemies = false;
        
        // Show game over screen
        if(gameOverScreen != null) {
            gameOverScreen.Setup(currentScore);
        }
        
        // Optional: Play game over sound
        // SoundManager.StopMusic();
        // EXAMPLE: SoundManager.PlaySound(SoundType.GameOver, 1f);
        
        Debug.Log("Game Over - Player died with score: " + currentScore);
    }

}