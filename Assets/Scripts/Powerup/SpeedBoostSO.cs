using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Boost", menuName = "Game/Powerups/Speed Boost")]
public class SpeedBoostSO : PowerupSO
{
    public float SpeedMultiplier = 1.5f;
    public override void ApplyPowerup(GameObject target)
    {
        target.GetComponent<Entity>().MoveSpeed *= SpeedMultiplier;
    }

    public override void RemovePowerup(GameObject target)
    {
        target.GetComponent<Entity>().MoveSpeed /= SpeedMultiplier;
    }
}
