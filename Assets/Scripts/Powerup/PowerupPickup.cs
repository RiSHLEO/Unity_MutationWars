using Photon.Pun;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public PowerupSO Powerup;
    private PhotonView _photonView;
    [HideInInspector] public PowerupSpawner Spawner;
    [HideInInspector] public int SpawnPointIndex;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var powerupManager = other.GetComponent<PowerupManager>();

        if (powerupManager != null && Powerup != null)
        {
            powerupManager.PowerupApply(Powerup);
            _photonView.RPC(nameof(RequestDestroy), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Spawner?.NotifyPowerupCollected(SpawnPointIndex);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
