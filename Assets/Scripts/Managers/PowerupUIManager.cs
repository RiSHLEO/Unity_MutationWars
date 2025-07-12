using System.Collections.Generic;
using UnityEngine;

public class PowerupUIManager : MonoBehaviour
{
    public static PowerupUIManager Instance { get; private set; }

    [SerializeField] private Transform _iconHolder;
    [SerializeField] private GameObject _powerupIconPrefab;

    private Dictionary<string, GameObject> _activeIcons = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void ShowPowerupIcon(string key, Sprite icon)
    {
        if(_activeIcons.ContainsKey(key))
            return;

        GameObject powerupIcon = Instantiate(_powerupIconPrefab, _iconHolder);
        powerupIcon.GetComponent<PowerupIconUI>().SetIcon(icon);
        _activeIcons[key] = powerupIcon;
    }

    public void RemovePowerupIcon(string key)
    {
        if(_activeIcons.TryGetValue(key, out GameObject powerupIcon))        //check if value exists with this key in dict. if exists
                                                                             //get that gameobject and store it in powerupIcon
        {
            Destroy(powerupIcon);
            _activeIcons.Remove(key);
        }
    }
}
