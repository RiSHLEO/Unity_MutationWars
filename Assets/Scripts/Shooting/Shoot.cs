using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class Shoot : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private ShootingDataSO _shootingData;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private MutationHandler _mutationHandler;

    private float _nextTimeToFire = 0f;

    public PlayerInputSet InputSet { get; private set; }

    private void Awake()
    {
        InputSet = new PlayerInputSet();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        InputSet.Enable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        InputSet.Disable();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 direction = (mousePos - centerPoint.position).normalized;
        transform.position = centerPoint.position + direction * radius;
        _firePoint.right = direction;

        if (InputSet.Player.Attack.WasPerformedThisFrame() && Time.time >= _nextTimeToFire)
        {
            Shooting();
            _nextTimeToFire = Time.time + _shootingData.FireRate;
        }
    }

    private void Shooting()
    {
        switch (_shootingData.shootingType)
        {
            case ShootingType.Standard:
                FireBullet(_firePoint.right);
                break;

            case ShootingType.Shotgun:
                float baseAngle = _firePoint.rotation.eulerAngles.z;
                int count = _shootingData.BulletsPerShot;
                float spread = _shootingData.SpreadAngle;

                for (int i = 0; i < count; i++)
                {
                    float angle = baseAngle + Random.Range(-spread / 2f, spread / 2f);
                    Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
                    FireBullet(direction.normalized);
                }
                break;
        }
    }

    private void FireBullet(Vector3 direction)
    {
        Vector3 startPos = transform.position;
        GameObject bullet = Instantiate(_bulletPrefab, startPos, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * _shootingData.BulletSpeed;
        photonView.RPC(nameof(_mutationHandler.SpawnBulletInOther), RpcTarget.Others, startPos, direction, (float)PhotonNetwork.Time);
    }
}
