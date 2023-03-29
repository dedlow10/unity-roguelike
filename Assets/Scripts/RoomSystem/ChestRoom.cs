using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EnemyRoom;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ChestRoom : RoomType
{
    [SerializeField] GameObject chest;
    [SerializeField]
    List<BaseItem> items;
    public override void GenerateRoom(int roomLevel, Transform transform)
    {
        int index = Random.Range(0, items.Count);
        var item = items[index];


        var chestGameObject = Instantiate(chest, transform.position, Quaternion.identity);
        chestGameObject.GetComponent<Chest>().AddItem(item);
    }
}
