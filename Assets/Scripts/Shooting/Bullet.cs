using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private ShootingDataSO _shootingData;

    private Vector3 _startPosition;
    private float _range;

    private void Start()
    {
        _startPosition = transform.position;
        _range = _shootingData.Range;
    }

    private void Update()
    {
        float distanceTravelled = Vector3.Distance(_startPosition, transform.position);

        if (distanceTravelled > _range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();

            if (targetPhotonView != null && !targetPhotonView.IsMine)
            {
                targetPhotonView.RPC("ReduceXPOnHit", targetPhotonView.Owner, 10);
            }
        }

        //Destroy(gameObject);
    }
}
