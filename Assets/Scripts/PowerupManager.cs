using Inventory.Model;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance { get; private set; }

    [SerializeField] GameObject PowerupUI;

    private Dictionary<string, ActivePowerup> currentPowerups = new Dictionary<string, ActivePowerup>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        foreach(var powerup in currentPowerups.ToList())
        {
            var timeRemaining = currentPowerups[powerup.Key].timeRemaining - Time.deltaTime;
            currentPowerups[powerup.Key].timeRemaining = timeRemaining;

            if (timeRemaining <= 0) {
                DisablePowerup(powerup.Key);
            }
        }
    }

    public bool EnablePowerup(string name, PowerupSO powerup)
    {
        if (!currentPowerups.ContainsKey(name))
        {
            var ui = PowerupUI;
            var image = ui.GetComponentInChildren<Image>();
            image.sprite = powerup.ItemImage;
            var newEl = Instantiate(ui, transform);

            currentPowerups.Add(name, new ActivePowerup(powerup, newEl));
            powerup.OnEnablePowerup();


            return true;
        }

        return false;
    }

    public void DisablePowerup(string name)
    {
        var powerup = currentPowerups[name];
        if (powerup != null)
        {
            currentPowerups.Remove(name);
            powerup.powerupSO.OnDisablePowerup();
            Destroy(powerup.uiEl);
        }
    }

    public void DisableAllWeaponPowerups()
    {
        foreach(var powerup in currentPowerups.ToList())
        {
            if (powerup.Value.powerupSO.IsWeaponPowerup())
            {
                DisablePowerup(powerup.Key);
            }
        }
    }

    public class ActivePowerup
    {
        public PowerupSO powerupSO;
        public float timeRemaining;
        public GameObject uiEl;
        public ActivePowerup(PowerupSO powerupSO, GameObject uiEl)
        {
            this.powerupSO = powerupSO;
            this.timeRemaining = powerupSO.timeLength;
            this.uiEl = uiEl;
        }
    }
}
