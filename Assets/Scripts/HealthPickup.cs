using UnityEngine;

public class HealthPickup : BaseItem
{
    [SerializeField] int health;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.GetPlayer().RestoreHealth(health);
            AudioManager.instance.PlaySFX("ItemPickup", transform.position);
            Destroy(gameObject);
        }
    }

    public override string GetName()
    {
        return "+" + health + " health";
    }
    public override void ReceiveItem()
    {
        GameManager.instance.GetPlayer().RestoreHealth(health);
    }
}
