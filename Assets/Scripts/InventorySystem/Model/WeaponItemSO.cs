using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class WeaponItemSO : ItemSO, IDroppableItem, IItemAction
    {
     
        [SerializeField] int maxAmmo;
        int currentAmmo;

        private void OnEnable()
        {
            currentAmmo = maxAmmo;
        }

        public int GetCurrentAmmo()
        {
            return currentAmmo;
        }

        public int GetMaxAmmo()
        {
            return maxAmmo;
        }

        public string ActionName => "Equip";
        public Weapon weapon;

        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            GameManager.instance.GetPlayer().EquipWeapon(this);
            return true;
        }

        public void SetCurrentAmmo(int amount)
        {
            currentAmmo = Math.Min(amount, maxAmmo);
        }
    }
}