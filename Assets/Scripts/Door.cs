using Inventory;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animatorController;
    [SerializeField] bool isOpen = false;
    [SerializeField] bool isUnlocked = true;
    [SerializeField] ItemSO key;

    private void Start()
    {
        if (isOpen)
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (!isUnlocked && PlayerHasKey()) 
            {
                isUnlocked = true;
            }

            if (isUnlocked)
            {
                if (!isOpen)
                {
                    OpenDoor();
                }
            }
            else
            {
                DialogueManager.instance.ShowText(key.Name + " is needed in order to unlock this door.");
            }
        }
        else if (collision.tag == "Enemy")
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" || collision.tag == "Enemy")
        {
            StartCoroutine(CloseDoorEventually());
        }
    }

    private IEnumerator CloseDoorEventually()
    {
        yield return new WaitForSeconds(2);
        if (isOpen)
        {
            CloseDoor();
        }
    }

    private IEnumerator DelayDoorOpenComplete()
    {
        yield return new WaitForSeconds(.3f);
        DoorOpenComplete();
    }

    private void OpenDoor()
    {
        if (animatorController != null)
        {
            animatorController.SetTrigger("Open");
        }
        else
        {
            StartCoroutine(DelayDoorOpenComplete());
        }

        isOpen = true;
    }

    public void DoorOpenComplete()
    {
        transform.Find("DoorClosed").gameObject.SetActive(false);
        transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        AstarPath.active.UpdateGraphs(gameObject.GetComponent<Renderer>().bounds);
    }

    private void CloseDoor()
    {
        if (animatorController != null)
        {
            animatorController.SetTrigger("Close");
        }
        isOpen = false;
        transform.Find("DoorClosed").gameObject.SetActive(true);
        transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        AstarPath.active.UpdateGraphs(gameObject.GetComponent<Renderer>().bounds);
    }

    private bool PlayerHasKey()
    {
        var inventoryController = GameManager.instance.GetPlayer().GetComponent<InventoryController>();
        var items = inventoryController.FindItems(key.Name);
        return items.Count > 0;
    }
}
