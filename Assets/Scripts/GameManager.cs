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
            enemySpawnTimer = 0f; // Reset timer
        }
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(-4f, 4f); // Random X between -4 and 4
        Vector2 spawnPosition = new Vector2(randomX, 7f);
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(GameObject.Find("Enemies").transform);

        // Randomly assign enemy type
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.enemyType = (EnemyController.EnemyType)Random.Range(0, 2);
            Debug.Log("Enemy Type: " + enemyController.enemyType);
        }
    }
}
