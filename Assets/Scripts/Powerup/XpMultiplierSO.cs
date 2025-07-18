using UnityEngine;

[CreateAssetMenu(fileName = "New Xp Boost", menuName = "Game/Powerups/XP Boost")]
public class XpMultiplierSO : PowerupSO
{
    [SerializeField] private float _xpMultiplier = 1.5f;

    public override void ApplyPowerup(GameObject target)
    {
        target.GetComponent<MutationHandler>()._xpAmount *= _xpMultiplier;
    }

    public override void RemovePowerup(GameObject target)
    {
        target.GetComponent<MutationHandler>()._xpAmount /= _xpMultiplier;
    }
}
