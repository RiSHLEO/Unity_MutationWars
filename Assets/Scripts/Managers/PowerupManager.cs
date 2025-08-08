using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private TextMeshProUGUI _powerupName;

    private class ActivePowerupData
    {
        public Coroutine Coroutine;
        public PowerupSO Powerup;
    }

    private Dictionary<string, ActivePowerupData> _activePowerups = new();

    private void Awake()
    {
        if (_powerupName == null)
            _powerupName = GameObject.Find("PowerupNameText").GetComponent<TextMeshProUGUI>();
    }

    public void PowerupApply(PowerupSO powerup)
    {
        _powerupName.text = powerup.PowerupName;
        _powerupName.gameObject.SetActive(true);

        string key = powerup.PowerupName;

        if (_activePowerups.ContainsKey(key))
        {
            StopCoroutine(_activePowerups[key].Coroutine);
            powerup.RemovePowerup(gameObject);
            PowerupUIManager.Instance.RemovePowerupIcon(key);
        }

        powerup.ApplyPowerup(gameObject);
        PowerupUIManager.Instance.ShowPowerupIcon(key, powerup.Icon);
        Coroutine co = StartCoroutine(RemovePowerupAfterDuration(powerup));

        _activePowerups[key] = new ActivePowerupData
        {
            Powerup = powerup,
            Coroutine = co
        };
    }

    private IEnumerator RemovePowerupAfterDuration(PowerupSO powerup)
    {
        yield return new WaitForSeconds(powerup.Duration);
        _powerupName.gameObject.SetActive(false);
        powerup.RemovePowerup(gameObject);
        PowerupUIManager.Instance.RemovePowerupIcon(powerup.PowerupName);
        _activePowerups.Remove(powerup.PowerupName);
    }
}
