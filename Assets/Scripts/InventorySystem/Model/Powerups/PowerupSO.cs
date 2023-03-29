using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public abstract class PowerupSO : ItemSO, IDroppableItem, IItemAction, IDestroyableItem
    {
        public string ActionName => "Use";

        public float timeLength;

        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState)
        {
            var succeeded = PowerupManager.instance.EnablePowerup(Name, this);

            return succeeded;
        }

        public abstract void OnEnablePowerup();
        public abstract void OnDisablePowerup();
        public abstract bool IsWeaponPowerup();
    }
}