using UnityEngine;
public class playerController : MonoBehaviour
{

    public Rigidbody2D playerRigidbody;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        if(Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }

        playerRigidbody.linearVelocity = inputVector * 5;
    }
}
