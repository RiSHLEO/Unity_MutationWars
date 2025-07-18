using System.Collections;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneScoreManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _leaderboardUI;
    [SerializeField] private Transform _leaderboardHolder;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _returnButton;

    private double _startTime;
    private bool _isGameEnded = false;
    public float _gameDuration = 10f;

    private int _currentScore = 0;

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _startTime = PhotonNetwork.Time;
            Hashtable props = new Hashtable { { "StartTime", _startTime } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        StartCoroutine(WaitForStartTime());

        InvokeRepeating(nameof(UpdateLeaderboard), 0.5f, 0.5f);
        UpdateScoreText();
    }

    private void Update()
    {
        if (_isGameEnded)
        {
            _leaderboardUI.SetActive(true);
            return;
        }

        if (_startTime <= 0) return;

        CalculateTimer();
        _leaderboardUI.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private void CalculateTimer()
    {
        double elapsed = PhotonNetwork.Time - _startTime;
        int remaining = (int)Mathf.Max(0f, _gameDuration - (float)elapsed);
        _timerText.text = remaining.ToString();

        if (remaining <= 0f && !_isGameEnded)
            EndGame();
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

    private void EndGame()
    {
        _isGameEnded = true;
        _returnButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator WaitForStartTime()
    {
        while (PhotonNetwork.CurrentRoom == null)
            yield return null;

        while (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime") ||
               PhotonNetwork.CurrentRoom.CustomProperties["StartTime"] == null)
        {
            yield return null;
        }

        _startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
    }
}
