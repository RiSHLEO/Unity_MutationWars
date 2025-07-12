using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _roomCode = "Map1";
    [SerializeField] private GameObject _ConnectingScreen;
    [SerializeField] private Transform[] _spawnPoints;
    [Space]
    public readonly string[] _formPrefabNames = { "Stone", "Veil", "Blade" };
    private string _currentname;
    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Screen.SetResolution(800, 600, false);
    }

    public void ChangeName(string name)
    {
        _currentname = name;
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Joining Lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("Joining or creating room...");
        PhotonNetwork.JoinOrCreateRoom(_roomCode, null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room. Spawning Player...");
        _ConnectingScreen.SetActive(false);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        int spawnIndex = Mathf.Clamp(playerCount - 1, 0, playerCount -  1);
        Vector3 spawnPosition = _spawnPoints[spawnIndex].position;

        int formIndex = (playerCount - 1) % 3;

        string prefabName = _formPrefabNames[formIndex];
        GameObject playerPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
        PhotonNetwork.LocalPlayer.NickName = _currentname;

        Entity entity = playerPrefab.GetComponent<Entity>();

        if (entity.photonView.IsMine)
        {
            CameraFollowPlayer cameraFollow = FindFirstObjectByType<CameraFollowPlayer>();
            if (cameraFollow != null)
                cameraFollow.SetFollow(playerPrefab.transform);
        }

        entity.photonView.RPC(("AssignFormIndex"), RpcTarget.AllBuffered, formIndex);
    }
}
