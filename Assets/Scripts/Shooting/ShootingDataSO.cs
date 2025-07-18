using UnityEngine;
public enum ShootingType
{
    Standard,
    Shotgun,
    Homing,
    Piercing
}

[CreateAssetMenu(fileName = "New Shooting Data", menuName = "Game/Shooting Data")]
public class ShootingDataSO : ScriptableObject
{
    public GameObject BulletPrefab;
    public float FireRate = 0.5f;
    public float BulletSpeed = 10f;
    public float Damage = 10f;
    public float Range = 10f;

    public ShootingType shootingType = ShootingType.Standard;
    public int BulletsPerShot = 1;
    public float SpreadAngle = 15f;
}
