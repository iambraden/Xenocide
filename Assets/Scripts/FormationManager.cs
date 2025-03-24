using UnityEngine;
using System.Collections;

public class FormationManager : MonoBehaviour
{
    [Header("Formation Settings")]
    public GameObject enemyPrefab;
    public int rows = 3;
    public int columns = 5;
    public float xSpacing = 0.8f;
    public float ySpacing = 0.8f;
    public float moveDownSpeed = 2f;
    public float horizontalSpeed = 2f;
    public float xMin = -4.6f;
    public float xMax = 4.6f;
    public float respawnDelay = 3f;

    private bool isMovingDown = true;
    private bool movingRight = true;
    private Vector2 targetPosition;
    private bool isRespawning = false;

    void Start(){
        targetPosition = transform.position; // store inital position
        StartNewWave();
    }
    void StartNewWave(){
        // Reset position create new
        transform.position = new Vector2(targetPosition.x, 7f);
        GenerateFormation();
        StartCoroutine(MoveDownCoroutine());
    }
    void GenerateFormation(){
        // Clear remaining enemies
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }

        for (int row = 0; row < rows; row++){
            for (int col = 0; col < columns; col++){
                // centered grid positions
                float xOffset = (col - (columns - 1) / 2f) * xSpacing;
                float yOffset = (row - (rows - 1) / 2f) * ySpacing;
                Vector2 localPosition = new Vector2(xOffset, yOffset);

                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemy.transform.localPosition = localPosition;

                // Set enemy type Sway
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null){
                    enemyController.enemyType = EnemyController.EnemyType.Sway;
                }
            }
        }
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
        isMovingDown = false;
    }

    void Update(){
        if (!isMovingDown){
            HandleHorizontalMovement();
        }

        if (transform.childCount == 0 && !isRespawning){
            StartCoroutine(RespawnWave());
        }
    }
    IEnumerator RespawnWave(){
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        
        isMovingDown = true;
        movingRight = true;
        
        StartNewWave();
        isRespawning = false;
    }

    void HandleHorizontalMovement(){
        // Move formation
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * horizontalSpeed * Time.deltaTime);

        // Check screen edges
        bool hitEdge = false;
        foreach (Transform enemy in transform){
            Vector2 enemyPos = enemy.position;
            if (movingRight && enemyPos.x >= xMax){
                hitEdge = true;
                break;
            }
            else if (!movingRight && enemyPos.x <= xMin){
                hitEdge = true;
                break;
            }
        }

        // Reverse direction if edge hit
        if (hitEdge){
            movingRight = !movingRight;
            transform.Translate(Vector2.down * 0.25f);
        }
    }
}