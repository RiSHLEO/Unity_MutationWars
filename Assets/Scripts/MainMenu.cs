using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ActivateSceneCards()
    {

    }

    public void StartSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayerScene");
    }

    public void StartMultiPlayer()
    {
        SceneManager.LoadScene("MultiPlayerScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
