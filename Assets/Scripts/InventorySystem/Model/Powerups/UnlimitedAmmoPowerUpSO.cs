using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class UnlimitedAmmoPowerupSO
        : PowerupSO
    {
        float originalCooldown;
        [SerializeField] float cooldown;

        public override void OnEnablePowerup()
        {
            var aim = GameManager.instance.GetPlayer().GetComponentInChildren<PlayerAimWeapon>();
            aim.ignoreAmmo = true;

        }

        public override void OnDisablePowerup()
        {
            var aim = GameManager.instance.GetPlayer().GetComponentInChildren<PlayerAimWeapon>();
            aim.ignoreAmmo = false;
        }

        public override bool IsWeaponPowerup()
        {
            return true;
        }

    }
}