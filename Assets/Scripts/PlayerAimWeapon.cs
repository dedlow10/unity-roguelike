using System;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndpointTransform;
    public event EventHandler<OnShootEventArgs> OnShoot;
    private SpriteRenderer gunSprite;
    private ParticleSystem gunSmoke;
    private GameObject equippedWeapon;
    private Weapon equippedWeaponWeapon;
    private string firingSoundFx = "Laser2";
    public bool ignoreAmmo = false;

    public class OnShootEventArgs: EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Transform gunEndPointTransform;
        public Vector3 shootPosition;
    }

    public void SetWeapon(GameObject gun)
    {
        if (equippedWeapon == null)
        {
            equippedWeapon = Instantiate(gun, aimTransform);
        }
        else
        {
            equippedWeapon = ReplaceChildObject(equippedWeapon.transform.parent.gameObject, equippedWeapon, gun);
        }

        gunSprite = equippedWeapon.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        aimGunEndpointTransform = equippedWeapon.transform.Find("GunEndpointPosition");
        gunSmoke = equippedWeapon.GetComponentInChildren<ParticleSystem>();
        equippedWeaponWeapon = equippedWeapon.GetComponent<Weapon>();
    }

    // Call this method to replace the child gameObject
    public GameObject ReplaceChildObject(GameObject parentObject, GameObject originalChildObject, GameObject replacementObject)
    {
        // Instantiate the replacement child gameObject as a child of the parent
        GameObject newChildObject = Instantiate(replacementObject, parentObject.transform);

        // Set the position and rotation of the replacement child to match the original child
        newChildObject.transform.position = originalChildObject.transform.position;
        newChildObject.transform.rotation = originalChildObject.transform.rotation;

        // Destroy the original child gameObject
        Destroy(originalChildObject);

        return newChildObject;
    }

    /*
    public void ChangeAmmo(GameObject ammo)
    {
        //equippedWeapon.GetComponent<Weapon>().bulletPrefab = ammo;
    }
    */

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    public bool HasWeapon()
    {
        return equippedWeapon != null;
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused()) return;
        if (!HasWeapon()) return;
  
        HandleAiming();


        if (equippedWeaponWeapon != null && equippedWeaponWeapon.CanFire() && (ignoreAmmo || GameManager.instance.GetPlayer().EquippedWeapon?.GetCurrentAmmo() > 0))
        {
            HandleShooting();
        }
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = MouseUtil.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        if (aimDirection.x < 0)
        {
            gunSprite.flipY = true;
            if (gunSmoke.transform.localScale.y > 0)
            {
                gunSmoke.transform.localScale = new Vector3(gunSmoke.transform.localScale.x, gunSmoke.transform.localScale.y * -1, gunSmoke.transform.localScale.z);
            }
        }
        else
        {
            gunSprite.flipY = false;
            if (gunSmoke.transform.localScale.y < 0)
            {
                gunSmoke.transform.localScale = new Vector3(gunSmoke.transform.localScale.x, gunSmoke.transform.localScale.y * -1, gunSmoke.transform.localScale.z);
            }

        }
    }
    private void HandleShooting()
    {
        var isFiring = !equippedWeaponWeapon.isAutomatic ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (isFiring)
        {
            AudioManager.instance.PlaySFX(firingSoundFx, aimGunEndpointTransform.position);
            if (gunSprite.flipX)
            {
                gunSmoke.startRotation = aimGunEndpointTransform.rotation.eulerAngles.z * Mathf.Deg2Rad;

            }
            else
            {
                gunSmoke.startRotation = aimGunEndpointTransform.rotation.eulerAngles.z * -1 * Mathf.Deg2Rad;
            }


            if (!ignoreAmmo) UpdateAmmo();

            gunSmoke.Play();
            Vector3 mousePosition = MouseUtil.GetMouseWorldPosition();
            OnShoot.Invoke(this, new OnShootEventArgs { gunEndPointPosition = aimGunEndpointTransform.position, shootPosition = mousePosition, gunEndPointTransform = aimGunEndpointTransform });
        }
    }

    private void UpdateAmmo()
    {
        var player = GameManager.instance.GetPlayer();
        player.EquippedWeapon.SetCurrentAmmo(player.EquippedWeapon.GetCurrentAmmo() - 1);
        var weaponPanel = GameManager.instance.WeaponPanel;
        weaponPanel.UpdateAmmoRemaining(player.EquippedWeapon?.GetCurrentAmmo() + " / " + player.EquippedWeapon.GetMaxAmmo());
    }
}
