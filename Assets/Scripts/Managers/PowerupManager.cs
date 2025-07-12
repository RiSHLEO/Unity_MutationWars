using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private class ActivePowerupData
    {
        public Coroutine Coroutine;
        public PowerupSO Powerup;
    }

    private Dictionary<string, ActivePowerupData> _activePowerups = new();

    public void PowerupApply(PowerupSO powerup)
    {
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
        powerup.RemovePowerup(gameObject);
        PowerupUIManager.Instance.RemovePowerupIcon(powerup.PowerupName);
        _activePowerups.Remove(powerup.PowerupName);
    }
}
