using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerOpenChest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var playerTransform = GameManager.instance.GetPlayer().transform;
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, playerTransform.forward);
            if (hit.collider.tag == "Chest")
            {
                hit.collider.gameObject.GetComponent<Chest>().Open();
            }
        }
    }
}
