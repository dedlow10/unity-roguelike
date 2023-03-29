using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            AudioManager.instance.PlaySFX("ItemPickup", transform.position);
            FloatingTextManager.instance.Show("Found " + item.InventoryItem.Name, 25, Color.green, transform.position, Vector3.up * 30, 2.0f);
            if (reminder == 0)
                item.DestroyItem();
            else
                item.Quantity = reminder;
        }
    }

    public void AddItem(ItemSO item)
    {
        inventoryData.AddItem(item, 1);
    }
}
