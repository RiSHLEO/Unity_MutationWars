using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float MatchDuration = 60f;
    private float _remainingTime;

    [Header("Score")]
    public int Score = 0;
    [SerializeField] private float _scoreInterval = 3f;
    private Coroutine _scoreCoroutine;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    private bool gameOver = false;

    private void Start()
    {
        _remainingTime = MatchDuration;
        UpdateUI();

        _scoreCoroutine = StartCoroutine(PassiveScoreGain());
    }

    private void Update()
    {
        if (gameOver) return;

        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0)
        {
            EndGame();
        }

        UpdateUI();
    }

    private IEnumerator PassiveScoreGain()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(_scoreInterval);
            Score += 1;
            UpdateUI();
        }
    }

    public void PlayerHit()
    {
        Score -= 5;
        if (Score < 0) Score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_timerText != null)
            _timerText.text = "Time: " + Mathf.CeilToInt(_remainingTime).ToString();

        if (_scoreText != null)
            _scoreText.text = "Score: " + Score.ToString();
    }

    private void EndGame()
    {
        gameOver = true;

        if (_scoreCoroutine != null)
            StopCoroutine(_scoreCoroutine);

        _restartButton.SetActive(true);
        Time.timeScale = 0f;
        _finalScoreText.text = "Final Score: " + Score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
