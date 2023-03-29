using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float DestroyAfter = 0;
    [SerializeField] GameObject ImpactEffect;
    [SerializeField] string targetTag;

    private int damage = 1;
    private bool hasCollided = false;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(DestroyEventually(5));
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Update()
    {
        LookVelocity();
    }

    public void LookVelocity()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject;

        if (collisionObject.layer == LayerMask.NameToLayer("Obstacle") || collisionObject.layer == LayerMask.NameToLayer("Object"))
        {
            Destroy(gameObject);
            return;
        }

        if (collisionObject.tag == targetTag && !hasCollided)
        {
            hasCollided = true;
            if (ImpactEffect != null)
            {
                ContactPoint2D contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                rot.x = -90f;
                Vector3 pos = contact.point;
                Debug.Log("Explosion Effect");
                Instantiate(ImpactEffect, pos, rot);

            }
            AudioManager.instance.PlaySFX("Hit", transform.position);
            collisionObject.GetComponent<Fighter>().ReceiveDamage(damage);
            StartCoroutine(DestroyEventually(DestroyAfter));
        }
    }

    IEnumerator DestroyEventually(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}