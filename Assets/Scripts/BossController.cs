using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Spawn")]
    private PlayerController playerController; // Reference to PlayerController
    public float moveDownSpeed = 1f;

    [Header("Shooting")]
    public Vector2 startPosition;

    void Start()
    {
        // Start the boss above the screen
        transform.position = new Vector2(transform.position.x, 7f);
    
        // Find the player and disable their shooting
        playerController = FindObjectOfType<PlayerController>();
        playerController.canShoot = false; // Disable shooting

        startPosition = new Vector2(transform.position.x, 3.5f);
        StartCoroutine(MoveBossDownCoroutine());
    }

    void Update()
    {
        // Boss behavior logic here
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
    }
}
