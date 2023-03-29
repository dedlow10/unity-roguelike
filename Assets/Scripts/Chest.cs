using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite ChestOpenSprite;
    [SerializeReference] private List<BaseItem> chestItems;
    [SerializeField] private KeyCode keyCode = KeyCode.E;

    private SpriteRenderer spriteRenderer;

    private bool isPlayerColliding = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Open()
    {
        AudioManager.instance.PlaySFX("ChestOpen", transform.position);
        spriteRenderer.sprite = ChestOpenSprite;
        var items = string.Join(" and ", chestItems.Select(ci => ci.GetName()).ToArray());
        foreach(var item in chestItems) {
            item.ReceiveItem();
        }
        FloatingTextManager.instance.Show("Found " + items, 25, Color.green, transform.position, Vector3.up * 30, 2.0f);
    }

    private void Update()
    {
        if (isPlayerColliding && Input.GetKeyDown(keyCode))
        {
            Open();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerColliding = false;
        }
    }

    public void AddItem(BaseItem item)
    {
        chestItems.Add(item);
    }
}
