using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int quantity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CoinManager.instance.AddCoins(quantity);
            AudioManager.instance.PlaySFX("ItemPickup", transform.position);
            Destroy(gameObject);
        }
    }

    
}
