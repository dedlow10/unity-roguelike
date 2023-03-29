using Pathfinding.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class EnemyRoom : RoomType
{
    [SerializeField] List<EnemyGrouping> enemyGrouping;

    [SerializeField]
    public override void GenerateRoom(int roomLevel, Transform transform)
    {
        int index = Random.Range(0, enemyGrouping.Count);
        var grouping = enemyGrouping[index];
        foreach(var enemyQuantity in grouping.enemies)
        {
            for(int x = 0; x < enemyQuantity.quantity; x++)
            {
                var randomX = Random.Range(-2, 2);
                var randomY = Random.Range(-2, 2);
                var position = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);

                Instantiate(enemyQuantity.enemy.gameObject, position, transform.rotation);
            }
        }
    }

    [Serializable]
    public class EnemyGrouping
    {
        public List<EnemyQuantity> enemies;
    }

    [Serializable]
    public class EnemyQuantity
    {
        public int quantity;
        public Enemy enemy;
    }
}
