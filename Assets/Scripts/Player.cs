using Inventory.Model;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Player : Fighter
{
    private float InvulnerabilityTime = 3f;
    private float timeSinceHit = 99f;

    public static Player instance { get; private set; }

    public WeaponItemSO EquippedWeapon;
    //public AmmoItemSO EquippedAmmo;
    public Texture2D CursorTexture;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void EquipWeapon(WeaponItemSO item)
    {
        GetComponent<PlayerAimWeapon>().SetWeapon(item.weapon.gameObject);
        GameManager.instance.WeaponPanel.SetActiveWeapon(item.ItemImage);
        //GameManager.instance.WeaponPanel.UpdateAmmoRemaining(item.weapon.CurrentAmmo + " / " + item.weapon.MaxAmmo);
        EquippedWeapon = item;
        GameManager.instance.WeaponPanel.UpdateAmmoRemaining(item.GetCurrentAmmo() + " / " + item.GetMaxAmmo());
        PowerupManager.instance.DisableAllWeaponPowerups();
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
    }

    //public void EquipAmmo(AmmoItemSO item)
    //{
        //GetComponent<PlayerAimWeapon>().Set(item.weapon.gameObject);
        //GameManager.instance.WeaponPanel.SetActiveWeapon(item.ItemImage);
        //GameManager.instance.WeaponPanel.UpdateAmmoRemaining(item.GetCurrentAmmo() + " / " + item.GetMaxAmmo().value);
        //EquippedAmmo = item;
        //GetComponent<PlayerAimWeapon>().ChangeAmmo(EquippedAmmo.projectile);
    //}

    private void Update()
    {
       timeSinceHit += Time.deltaTime;
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        GameManager.instance.UpdateHealthBar();

        if (Hitpoints <= 0) { GameManager.instance.ShowGameOverScreen(); }
    }

    public override void RestoreHealth(int health)
    {
        base.RestoreHealth(health);

        GameManager.instance.UpdateHealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject;


        if (collisionObject.tag == "Enemy" && timeSinceHit >= InvulnerabilityTime)
        {
            timeSinceHit = 0;
            ReceiveDamage(1);

            var rb = gameObject.GetComponent<Rigidbody2D>();
            var player = gameObject.GetComponent<PlayerController>();
            Vector2 pushDirection = rb.transform.position - collisionObject.transform.position;
            player.OnPush(pushDirection * 1000f);
        }
    }
}
