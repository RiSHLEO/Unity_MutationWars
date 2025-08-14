using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayfabLoginManager : MonoBehaviour
{
    private const string LAST_EMAIL_KEY = "LAST_EMAIL", LAST_PASSWORD_KEY = "LAST_PASSWORD";

    [Header("Resgister UI")]
    [SerializeField] private TMP_InputField _registerEmail;
    [SerializeField] private TMP_InputField _registerUsername;
    [SerializeField] private TMP_InputField _registerPassword;

    [Header("Login UI")]
    [SerializeField] private TMP_InputField _loginEmail;
    [SerializeField] private TMP_InputField _loginPassword;

    [Header("Buttons")]
    [SerializeField] private GameObject _SinglePlayerButton;
    [SerializeField] private GameObject _MultiPlayerButton;
    [SerializeField] private GameObject _loginUI;
    [SerializeField] private GameObject _registerUI;


    public void OnRegisterPressed()
    {
        Register(_registerEmail.text, _registerUsername.text, _registerPassword.text);
    }

    private void Register(string email, string username, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
        {
            Email = email,
            DisplayName = username,
            Password = password,
            RequireBothUsernameAndEmail = false
        },
        successResult => Login(email, password),
        PlayfabFailure);
    }

    public void OnLoginPressed()
    {
        Login(_loginEmail.text, _loginPassword.text);
    }

    private void Login(string email, string password)
    {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest()
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        },
        successResult =>
        {
            PlayerPrefs.SetString(LAST_EMAIL_KEY, email);
            PlayerPrefs.SetString(LAST_PASSWORD_KEY, password);
            PlayerPrefs.SetString("Username", successResult.InfoResultPayload.PlayerProfile.DisplayName);

            Debug.Log("Login in successful" + PlayerPrefs.GetString("Username"));

            _registerUI.SetActive(false);
            _loginUI.SetActive(false);
            _SinglePlayerButton.SetActive(true);
            _MultiPlayerButton.SetActive(true);
        },
        PlayfabFailure);
    }

    private void PlayfabFailure(PlayFabError error)
    {
        Debug.Log(error.Error + " : " + error.GenerateErrorReport());
    }
}
