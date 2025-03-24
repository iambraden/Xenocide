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
    public float horizontalSpeed = 2f;
    public float xMin = -4.0f;
    public float xMax = 4.0f;

    

    void Start()
    {
        // Start the boss above the screen
        transform.position = new Vector2(transform.position.x, transform.position.y);
    
        // Find the player and disable their shooting
        playerController = FindObjectOfType<PlayerController>();
        playerController.canShoot = false; // Disable shooting

        startPosition = new Vector2(transform.position.x, 3.5f);
        StartCoroutine(MoveBossDownCoroutine());
    }

    void Update()
    {
        if(!isMovingDown)
        {
            Move();
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

        if(hitEdge)
        {
            movingRight = !movingRight;
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
    }
}
