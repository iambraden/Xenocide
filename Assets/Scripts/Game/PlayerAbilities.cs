using UnityEngine;
public class PlayerAbilities : MonoBehaviour
{
    public string[] Abilities =
    {
        "Dash",
        "Twinshot",
        "HealthIncrease",
        "PlayerSpeed",
        "BulletSpeed",
        "FireRate"
    };

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found.");
        }
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found.");
        }
    }

    public void ActivateAbility(string ability){
        switch(ability){
        case "Dash":
            playerController.setCanDash();
            break;
        case "Twinshot":
            playerController.setTwinShot();
            break;
        case "HealthIncrease":
            playerHealth.HealPlayer();
            break;
        case "PlayerSpeed":
            playerController.IncreaseMoveSpeed();
            break;
        case "BulletSpeed":
            playerController.IncreaseBulletSpeed();
            break;
        case "FireRate":
            playerController.IncreaseFireRate();
            break;
        }
    }
}
