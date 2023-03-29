using UnityEngine;

public class AmmoRecharge : MonoBehaviour
{
    [SerializeField] int amount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var weapon = GameManager.instance.GetPlayer().EquippedWeapon;
        if (weapon != null && collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySFX("ItemPickup", transform.position);
            weapon.SetCurrentAmmo(weapon.GetCurrentAmmo() + amount);
            GameManager.instance.WeaponPanel.UpdateAmmoRemaining(weapon?.GetCurrentAmmo() + " / " + weapon.GetMaxAmmo());
            Destroy(gameObject);
        }
    }
}
