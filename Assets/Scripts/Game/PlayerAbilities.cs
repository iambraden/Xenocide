using UnityEngine;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{
    public List<string> Abilities = new List<string>
    {
        "Dash (Left Shift)",
        "Twinshot",
        "Health Increase",
        "+Player Speed",
        "+Bullet Speed",
        "+FireRate",
        "Force Field (30s cooldown)"
    };

    public PlayerController playerController;
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

    public void ActivateAbility(string ability)
    {
        switch (ability)
        {
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
                if (!Abilities.Contains("Force Field Upgrade"))
                {
                    Abilities.Add("Force Field Upgrade");
                    //Debug.Log("Added Force Field Upgrade to abilities.");
                }
                break;
            case "Force Field Upgrade":
                playerController.UpgradeForceField();
                break;
        }
    }
}
