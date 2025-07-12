using Photon.Pun;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using System.Linq;
using TMPro;
using System;

public class SceneScoreManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _leaderboardUI;
    [SerializeField] private Transform _leaderboardHolder;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _currentScore = 0;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateLeaderboard), 0.5f, 0.5f);
        UpdateScoreText();
    }


    private void Update()
    {
        _leaderboardUI.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private void UpdateLeaderboard()
    {
        for (int i = _leaderboardHolder.childCount - 1; i >= 0; i--)
            Destroy(_leaderboardHolder.GetChild(i).gameObject);

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        foreach (var player in sortedPlayerList)
        {
            GameObject playerItemPrefab = Instantiate(_playerItemPrefab, _leaderboardHolder);
            playerItemPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.NickName;
            playerItemPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.GetScore().ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player player, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;

        if (player == PhotonNetwork.LocalPlayer)
        {
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        int newScore = PhotonNetwork.LocalPlayer.GetScore();
        if (newScore != _currentScore)
        {
            _currentScore = newScore;
            _scoreText.text = "Score: " + _currentScore;
        }
    }
}
