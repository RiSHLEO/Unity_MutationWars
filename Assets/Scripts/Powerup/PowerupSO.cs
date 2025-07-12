using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Powerup", menuName = "Game/Powerup")]
public abstract class PowerupSO : ScriptableObject
{
    public string PowerupName;
    public Sprite Icon;
    public float Duration = 10f;

    public abstract void ApplyPowerup(GameObject target);
    public abstract void RemovePowerup(GameObject target);
}
