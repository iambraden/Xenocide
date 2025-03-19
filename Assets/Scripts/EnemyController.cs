using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 startPos;

    public float maxDistance = 5f;
    public float frequency = 2f;
    public float phaseOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI); // Random phase offset for variation
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        if (pos.y < -5.5f)
        {
            pos.y = 5.5f;
        }

        pos.y = pos.y - Time.deltaTime * speed;
        // Calculate sine wave motion around the initial x-position
        pos.x = startPos.x + Mathf.Sin((pos.y * frequency) + phaseOffset) * 2;
        transform.position = pos;
    }
}