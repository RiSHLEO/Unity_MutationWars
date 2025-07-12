using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections;


public class MutationHandler : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    private XPManager _xpmanager;
    private NetworkManager _networkmanager;

    [Header("Mutation Details")]
    public float _cooldown = 4f;
    private bool _mutationReady = true;

    [Header("Form Data")]
    private int _currentFormIndex;

    private void Awake()
    {
        _xpmanager = XPManager.Instance;
        _networkmanager = NetworkManager.Instance;
    }

    private void Update()
    {
        if(photonView.IsMine && _mutationReady)
            OnPressMutate();
    }

    [PunRPC]
    public void AssignFormIndex(int formIndex)
    {
        _currentFormIndex = formIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        MutationHandler otherPlayer = collision.GetComponent<MutationHandler>();

        if (otherPlayer == null) return;

        Veil otherVeil = otherPlayer.GetComponent<Veil>();

        if(otherVeil != null && otherVeil.IsInvulnerable) return;

        if (Beats(_currentFormIndex, otherPlayer._currentFormIndex))
        {
            PhotonNetwork.LocalPlayer.AddScore(10);
            _xpmanager.AddEnergy(20);
            //otherPlayer.photonView.RPC(nameof(LoseEnergy), otherPlayer.photonView.Owner, 10);
            otherPlayer.photonView.RPC(nameof(MutateToForm), otherPlayer.photonView.Owner, _currentFormIndex, 10);
        }
    }

    bool Beats(int myIndex, int theirIndex)
    {
        // Stone(0) beats Blade(2)
        // Blade(2) beats Veil(1)
        // Veil(1) beats Stone(0)

        return (myIndex == 0 && theirIndex == 2)
            || (myIndex == 2 && theirIndex == 1)
            || (myIndex == 1 && theirIndex == 0);
    }

    private void OnPressMutate()
    {
        if (_xpmanager.currentEnergy == 0 || _mutationReady == false) return;

        int formIndex = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            formIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            formIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            formIndex = 2;

        if (formIndex != -1)
        {
            photonView.RPC(nameof(MutateToForm), RpcTarget.AllBuffered, formIndex, 10);
        }
    }

    [PunRPC]
    private void MutateToForm(int formIndex, int loseenergyAmount)
    {
        if (!photonView.IsMine) return;
        if (_currentFormIndex == formIndex) return;

        _xpmanager.SpendEnergy(loseenergyAmount);

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        PhotonNetwork.Destroy(gameObject);
        string newFormName = _networkmanager._formPrefabNames[formIndex];
        GameObject newForm = PhotonNetwork.Instantiate(newFormName, position, rotation, 0, new object[] { formIndex });

        if (newForm.GetComponent<PhotonView>().IsMine)
        {
            CameraFollowPlayer camFollow = FindFirstObjectByType<CameraFollowPlayer>();
            if (camFollow != null)
                camFollow.SetFollow(newForm.transform);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        if (instantiationData != null && instantiationData.Length > 0)
            _currentFormIndex = (int)instantiationData[0];

        StartCoroutine(AbilityCooldownRoutine(_cooldown));
    }

    [PunRPC]
    private void LoseEnergy(int amount)
    {
        _xpmanager.SpendEnergy(amount);
    }

    IEnumerator AbilityCooldownRoutine(float cooldown)
    {
        _mutationReady = false;
        yield return new WaitForSeconds(cooldown);
        _mutationReady = true;
    }
}
