using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class FireSpeedPowerupSO : PowerupSO
    {
        float originalCooldown;
        [SerializeField] float cooldown;
        
        public override void OnEnablePowerup()
        {
            var weapon = GameManager.instance.GetPlayer().GetComponentInChildren<Weapon>();
            originalCooldown = weapon.cooldown;
            weapon.cooldown = originalCooldown * .25f;
        }

        public override void OnDisablePowerup()
        {
            var weapon = GameManager.instance.GetPlayer().GetComponentInChildren<Weapon>();
            weapon.cooldown = originalCooldown;
        }

        public override bool IsWeaponPowerup()
        {
            return true;
        }
    }
}