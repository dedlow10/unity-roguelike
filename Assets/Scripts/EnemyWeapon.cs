using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] int volleySize = 1;
    [SerializeField] float cooldown = 3f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireSpeed = 1f;
    [SerializeField] GameObject firePoint;
    [SerializeField] float angleOfFireRandomness = 30f;

    private Transform aimTransform;

    private float cooldownCounter = 0;

    private void Awake()
    {
        aimTransform = transform.parent;
    }

    public void Fire()
    {
        if (gameObject != null)
        {
            for (int x = 0; x < volleySize; x++)
            {
                FireBulletRandomAngle();
            }

            AudioManager.instance.PlaySFX("EnemyLaser", transform.position, .2f);
            cooldownCounter = 0;
        }

    }

    private void FireBulletRandomAngle()
    {
        Vector3 aimDirection = (GameManager.instance.GetPlayer().transform.position - transform.position).normalized;
        float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) - 90;
        aimTransform.eulerAngles = new Vector3(0, 0, angle + Random.Range(-angleOfFireRandomness, angleOfFireRandomness));
        
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.transform.position, aimTransform.rotation);
        Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
        rb.AddForce(gameObject.transform.up * fireSpeed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        HandleAiming();
        cooldownCounter += Time.deltaTime;
    }

    public bool CanFire()
    {
        if (cooldownCounter > cooldown)
        {
            return true;
        }
        return false;
    }

    private void HandleAiming()
    {
        var player = GameManager.instance.GetPlayer();
        if (!player) return;

        Vector3 aimDirection = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle - 90);
    }
}
