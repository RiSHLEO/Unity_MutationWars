using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public int currentEnergy = 0;
    public int maxEnergy = 100;

    private float _timer = 0f;

    public static XPManager Instance { get; private set; }

    public int _xpPerInterval = 1;
    [SerializeField] private float _interval = 2;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image _xpFill;


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
        UpdateEnergyUI();
    }

    private void Update()
    {
        PassiveXP();
    }

    private void PassiveXP()
    {
        _timer += Time.deltaTime;

        if (_timer > _interval)
        {
            currentEnergy = Mathf.Min(currentEnergy + _xpPerInterval, maxEnergy);
            UpdateEnergyUI();
            _timer = 0f;
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyUI();
    }

    [PunRPC]
    public void SpendEnergy(int amount)
    {
        currentEnergy = Mathf.Max(currentEnergy - amount, 0);
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        float fillamount = (float)currentEnergy / maxEnergy;
        if (energyText != null)
        {
            energyText.text = $"Energy: {currentEnergy}/{maxEnergy}";
            _xpFill.fillAmount = fillamount;
        }
    }
}
