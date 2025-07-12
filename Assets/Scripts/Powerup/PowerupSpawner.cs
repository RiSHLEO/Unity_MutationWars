using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PowerupSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private Transform _powerupParent;
    [Space]
    [SerializeField] private GameObject[] _powerupPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    private Dictionary<int, GameObject> _activePowerups = new();

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnPowerupsLoop());
        }
    }

    private IEnumerator SpawnPowerupsLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (!PhotonNetwork.IsMasterClient)
                continue;

            TrySpawnPowerup();
        }
    }

    private void TrySpawnPowerup()
    {
        if (_activePowerups.Count >= _spawnPoints.Length)
            return;

        List<int> emptyPoints = new List<int>();

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (!_activePowerups.ContainsKey(i))
                emptyPoints.Add(i);
        }

        if (emptyPoints.Count == 0) return;

        int randomSpawnIndex = emptyPoints[Random.Range(0, emptyPoints.Count)];
        int randomPowerupIndex = Random.Range(0, _powerupPrefabs.Length);

        Vector3 spawnPosition = _spawnPoints[randomSpawnIndex].position;
        GameObject powerup = PhotonNetwork.Instantiate(_powerupPrefabs[randomPowerupIndex].name, spawnPosition, Quaternion.identity);
        powerup.transform.SetParent(_powerupParent);
        _activePowerups[randomSpawnIndex] = powerup;

        powerup.GetComponent<PowerupPickup>().Spawner = this;
        powerup.GetComponent<PowerupPickup>().SpawnPointIndex = randomSpawnIndex;
    }

    public void NotifyPowerupCollected(int spawnPointIndex)
    {
        if (_activePowerups.ContainsKey(spawnPointIndex))
            _activePowerups.Remove(spawnPointIndex);
    }
}
