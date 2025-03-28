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

    public void ActivateAbility(string ability){
        switch(ability){
        case "Dash":
            // Implement dash ability logic here
            break;
        case "Twinshot":
            // Implement twinshot ability logic here
            break;
        case "HealthIncrease":
            // Implement health increase ability logic here
            break;
        case "PlayerSpeed":
            // Implement player speed increase logic here
            break;
        case "BulletSpeed":
            // Implement bullet speed increase logic here
            break;
        case "FireRate":
            // Implement fire rate increase logic here
            break;
        }
    }
}
