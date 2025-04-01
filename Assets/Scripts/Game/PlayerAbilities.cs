using UnityEngine;
public class PlayerAbilities : MonoBehaviour
{
    public string[] Abilities =
    {
        "Dash (Left Shift)",
        "Twinshot",
        "Health Increase",
        "+Player Speed",
        "+Bullet Speed",
        "+FireRate"
    };

    public PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ActivateAbility(string ability){
        switch(ability){
        case "Dash (Left Shift)":
            playerController.setCanDash();
            break;
        case "Twinshot":
            playerController.setTwinShot();
            break;
        case "Heal + Health Increase (Max 5)":
            playerHealth.HealPlayer();
            break;
        case "+Player Speed":
            playerController.IncreaseMoveSpeed();
            break;
        case "+Bullet Speed":
            playerController.IncreaseBulletSpeed();
            break;
        case "+Fire Rate":
            playerController.IncreaseFireRate();
            break;
        case "Force Field (30s cooldown)":
            playerController.UnlockForceField();
            break;
        }
    }
}
