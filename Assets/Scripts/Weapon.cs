using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldown = 3f;
    public int damage = 1;
    public GameObject bulletPrefab;
    public bool isAutomatic = false;

    private PlayerAimWeapon playerAimWeapon;
    private float cooldownCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerAimWeapon = GameManager.instance.GetPlayer().GetComponent<PlayerAimWeapon>();
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;   
    }

    void Update()
    {
        cooldownCounter += Time.deltaTime;
    }

    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        if (gameObject != null)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, e.gunEndPointTransform.position, gameObject.transform.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetDamage(damage);
            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            rb.AddForce(gameObject.transform.up * 20f, ForceMode2D.Impulse);
            cooldownCounter = 0;
        }

    }

    public bool CanFire()
    {
        if (cooldownCounter > cooldown)
        {
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        playerAimWeapon.OnShoot -= PlayerAimWeapon_OnShoot;
    }
}
