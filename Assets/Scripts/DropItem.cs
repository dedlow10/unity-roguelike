using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropItem : MonoBehaviour
{
    [SerializeField] DropOption[] dropOptions;

    [Serializable]
    public class DropOption
    {
        public float chanceWeighted;
        public GameObject item;
    }

    private DropOption getRandomDrop()
    {
        var randomNum = Random.Range(0f, 1f);
        var total = dropOptions.Select(s => s.chanceWeighted).Sum();
        float startRange = 0;
        foreach (var option in dropOptions)
        {
            var chance = startRange + (option.chanceWeighted / total);
            if (randomNum <= chance)
            {
                return option;
            }
            else
            {
                startRange = startRange + chance;
            }
        }

        return null;
    }


    public void Drop()
    {
        var drop = getRandomDrop();

        Debug.Log("Dropping a: " + drop.item.name);
        Instantiate(drop.item, transform.position, Quaternion.identity);
    }
}
