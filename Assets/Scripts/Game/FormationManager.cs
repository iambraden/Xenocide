using UnityEngine;
using System.Collections;

public class FormationManager : MonoBehaviour
{
    [Header("Formation Settings")]
    public GameObject[] enemyPrefabs; // Possible enemy types
    public int rows = 3;
    public int columns = 5;
    public float xSpacing = 0.8f;
    public float ySpacing = 0.8f;
    public float moveDownSpeed = 2f;
    public float horizontalSpeed = 2f;
    public float xMin = -4.6f;
    public float xMax = 4.6f;

    [Header("Spawn Settings")]
    public float spawnChance = 0.5f; // Spawn rate
    private bool isMovingDown = true;
    private bool movingRight = true;
    private Vector2 targetPosition;

    // New wave movement and formation
    public void StartNewWave()
    {
        // Randomly determine the initial movement direction
        movingRight = Random.value > 0.5f;

        targetPosition = new Vector2(transform.position.x, 2.5f);
        GenerateFormation();
        StartCoroutine(MoveDownCoroutine());
    }

    // Create enemy grid with random empty spaces
    void GenerateFormation(){
        for (int row = 0; row < rows; row++){
            for (int col = 0; col < columns; col++){
                // Random skip based on spawn chance
                if (Random.value > spawnChance) continue;

                GameObject randomPrefab = GetRandomEnemyPrefab();

                // Calculate grid position with center offset
                float xOffset = (col - (columns - 1) / 2f) * xSpacing;
                float yOffset = (row - (rows - 1) / 2f) * ySpacing;
                Vector2 localPosition = new Vector2(xOffset, yOffset);

                // Create enemy and set position
                GameObject enemy = Instantiate(randomPrefab, transform);
                enemy.transform.localPosition = localPosition;

                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null){
                    enemyController.enemyType = EnemyController.EnemyType.Sway;
                }
            }
        }
    }

    // Random enemy prefab from array
    GameObject GetRandomEnemyPrefab(){
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    IEnumerator MoveDownCoroutine(){
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f){
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveDownSpeed * Time.deltaTime
            );
            yield return null;
        }
        isMovingDown = false; // Enable horizontal movement
    }

    void Update(){
        if (!isMovingDown) HandleHorizontalMovement();
    }

    void HandleHorizontalMovement(){
        // Set movement direction
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * horizontalSpeed * Time.deltaTime);

        // Check if any enemy reaches screen edge
        bool hitEdge = false;
        foreach (Transform enemy in transform){
            Vector2 enemyPos = enemy.position;
            if ((movingRight && enemyPos.x >= xMax) || (!movingRight && enemyPos.x <= xMin)){
                hitEdge = true;
                break;
            }
        }

        // Reverse direction and move down slightly on edge hit
        if (hitEdge){
            movingRight = !movingRight;
            transform.Translate(Vector2.down * 0.5f);
        }
    }

    public void IncreaseFormationDifficulty(int difficultyLevel){
        horizontalSpeed += 0.25f;
        spawnChance += 0.05f;

        if(difficultyLevel % 2 == 0 && difficultyLevel < 7){
            rows++;
            columns++;
        }
    }

    public void ResetDifficulty(){
        horizontalSpeed = 2f;
        spawnChance = 0.5f;
        rows = 3;
        columns = 5;
    }
}