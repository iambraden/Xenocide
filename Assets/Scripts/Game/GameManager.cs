using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public int difficulty = 0; 

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
    public GameObject boss;
    public GameObject warningSprite;
    private bool isBossActive = false;
    private int spawnBossScore = 2100; // Initial score required to spawn the first boss
    private int nextBossScoreIncrement = 2000; // Incremental score for the second boss
    private float bossSpawnCooldown = 5f; // Time to wait before resuming enemy spawning

    [Header("Game Over")]
    public GameOverScreen gameOverScreen;

    [Header("Score")]
    public int currentScore = 0;
    public TMPro.TextMeshProUGUI inGameScore;

    [Header("Upgrade System")]
    public GameObject upgradePrompt; // Reference to the UpgradePrompt GameObject
    public Button[] upgradeButtons; // Array of the 3 upgrade buttons
    public TMPro.TextMeshProUGUI[] upgradeButtonTexts; // Text components for the buttons
    private PlayerAbilities playerAbilities; // Reference to PlayerAbilities
    public bool isUpgradeActive = false;
    private int nextUpgradeScore = 500; // Score threshold for the next upgrade
    private HashSet<string> selectedAbilities = new HashSet<string>(); // Track selected abilities
    public bool ifDead = false; // Track if the player is dead

    void Start(){
        SoundManager.PlaySound(SoundType.GameMusic, 0.5f);
        waveTimer = waveInterval; // Start first wave after full interval

        playerAbilities = FindFirstObjectByType<PlayerAbilities>();

        upgradePrompt.SetActive(false); // Ensure the upgrade prompt is hidden at the start
    }

    void Update()
    {
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

        // Check if the player has reached the score threshold for an upgrade
        if (!isUpgradeActive && currentScore >= nextUpgradeScore && !ifDead)
        {
            StartCoroutine(ShowUpgradePrompt());
            if(difficulty < 5){
                nextUpgradeScore += 1500;
            }else{
                nextUpgradeScore += (difficulty - 3) * 1500;
            }
            
            
        }
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
    }

    public IEnumerator SpawnBossCoroutine(){
        // Stop enemy spawning temporarily
        isBossActive = true;
        canSpawnEnemies = false;

        SoundManager.PlaySound(SoundType.Warning);
        if (warningSprite != null){
            for (int i = 0; i < 4; i++){
                warningSprite.SetActive(true); // Enable the warning sprite
                yield return new WaitForSeconds(0.35f); 
                warningSprite.SetActive(false); // Disable the warning sprite
                yield return new WaitForSeconds(0.35f);
            }
        }

        // activate camera shake for boss entrance
        CameraShake.ShakeWithDuration(3.5f, 0.5f);

        // start boss music
        SoundManager.FadeOutMusic(1f);
        yield return new WaitForSeconds(1f);
        SoundManager.PlaySound(SoundType.BossMusic, 1.2f);

        // Spawn the boss at the top of the screen
        Vector2 bossSpawnPosition = new Vector2(0, 7f); // Centered at the top
        GameObject newBoss = Instantiate(boss, bossSpawnPosition, Quaternion.identity);
        newBoss.transform.SetParent(GameObject.Find("Enemies").transform);
    }

    public IEnumerator OnBossDefeatedCoroutine()
    {
        SoundManager.FadeOutMusic(1f);
        yield return new WaitForSeconds(1f);
        SoundManager.PlaySound(SoundType.GameMusic, 0.5f);
        //update score requirement, double increment for next boss spawn
        spawnBossScore = currentScore + nextBossScoreIncrement;
        nextBossScoreIncrement = Mathf.Min(5000, nextBossScoreIncrement * 2);

        isBossActive = false; // Reset the boss active flag
        UpdateDifficulty();
    }

    public void UpdateDifficulty()
    {
        difficulty++;
        //enemy spawn increases
        waveInterval = Mathf.Max(waveInterval *= 0.8f, 1f);
        enemySpawnInterval = Mathf.Max(enemySpawnInterval *= 0.8f, 0.25f);
        duoWaveSpawnChance *= 1.2f;
        formationPrefab.IncreaseFormationDifficulty(difficulty);
        bossActiveSpawnAdjustment = Mathf.Max(1f, bossActiveSpawnAdjustment * 0.9f); //boss shouldn't increase spawn rate
    }

    public void OnPlayerDeath()
    {
        // Stop gameplay
        canSpawnEnemies = false;
        ifDead = true;
        formationPrefab.ResetDifficulty();
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        playerController.ResetBulletSpeed();
        // Show game over screen
        if(gameOverScreen != null) {
            gameOverScreen.Setup(currentScore);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        inGameScore.text = "Score: " + currentScore.ToString();
        if(!isBossActive && currentScore >= spawnBossScore)
        {
            StartCoroutine(SpawnBossCoroutine());
        }
        
    }
    
    //getter for score (need it for pause screen and game over)
    public int getScore(){
        return this.currentScore;
    }

    private IEnumerator ShowUpgradePrompt()
    {
        isUpgradeActive = true;

        SoundManager.PlaySound(SoundType.PowerUp, 3f);

        Time.timeScale = 0f;

        upgradePrompt.SetActive(true);

        // Randomly select 3 unique abilities
        string[] abilities = playerAbilities.Abilities;
        List<string> availableAbilities = new List<string>(abilities);

        // Remove already-selected abilities from the pool
        availableAbilities.RemoveAll(ability => selectedAbilities.Contains(ability));

        string[] selectedAbilitiesArray = new string[3];

        for (int i = 0; i < 3; i++)
        {
            // Randomly pick an ability from the available list
            int randomIndex = Random.Range(0, availableAbilities.Count);
            selectedAbilitiesArray[i] = availableAbilities[randomIndex];

            // Remove the selected ability to prevent duplicates
            availableAbilities.RemoveAt(randomIndex);
        }

        // Update button texts and assign abilities
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i; 
            upgradeButtonTexts[i].text = selectedAbilitiesArray[i]; 
            upgradeButtons[i].onClick.RemoveAllListeners(); 
            upgradeButtons[i].onClick.AddListener(() =>
            {
                ActivateUpgrade(selectedAbilitiesArray[index]);
            });
        }

        // Wait for the player to select an upgrade
        while (isUpgradeActive)
        {
            yield return null;
        }
       
        Time.timeScale = 1f;
    }

    private void ActivateUpgrade(string ability)
    {
        // Activate the selected ability
        playerAbilities.ActivateAbility(ability);

        // If the selected ability is TwinShot or Dash, remove
        if (ability == "Twinshot" || ability == "Dash (Left Shift)" || ability == "Force Field (30s cooldown)")
        {
            selectedAbilities.Add(ability); // Add to the set of  removed abilities
        }

        upgradePrompt.SetActive(false);

        isUpgradeActive = false;
    }
}