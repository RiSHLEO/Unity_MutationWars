using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private ShootingDataSO _shootingData;

    private Vector3 _startPosition;
    private float _range;

    public void SetData(ShootingDataSO data)
    {
        _shootingData = data;
        _range = _shootingData.Range;
        _startPosition = transform.position;
    }

    private void Update()
    {
        float distanceTravelled = Vector3.Distance(_startPosition, transform.position);

        if (distanceTravelled > _range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 hitPosition = transform.position;
        PhotonNetwork.Instantiate("VFX_Hit", hitPosition, Quaternion.identity);

        if (other.CompareTag("Player"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();

            if (targetPhotonView != null && !targetPhotonView.IsMine)
            {
                targetPhotonView.RPC("ReduceXPOnHit", targetPhotonView.Owner, 10);
            }
        }

        Destroy(gameObject);
    }
}
