using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Enemy Spawn
    public GameObject enemy;
    public float enemySpawnInterval = 4f;
    private float enemySpawnTimer;

    void Update()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemySpawnTimer = 0f; // Reset timer on enemy spawn
        }
    }

    void SpawnEnemy()
    {
        //spawn enemies at a random x-position just above the screen
        float randomX = Random.Range(-4f, 4f);
        Vector2 spawnPosition = new Vector2(randomX, 7f);
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(GameObject.Find("Enemies").transform);

        // Randomly assign enemy type
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.enemyType = (EnemyController.EnemyType)Random.Range(0, 2);
            //TODO: implement sway enemy type
                //only make wave enemies for now
            enemyController.enemyType = EnemyController.EnemyType.Wave;
        }
    }
}
