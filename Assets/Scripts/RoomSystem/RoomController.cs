using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    bool hasPlayerEntered = false;
    public RoomType[] randomOptions;
    [SerializeField] int level;
    [SerializeField] Bounds bounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            //RemoveTilesWithinCollider(gameObject.GetComponent<BoxCollider2D>());
            if (hasPlayerEntered) return;
            hasPlayerEntered = true;

            GeneratorRandomOption(level);
        }
    }

    private RoomType getRandomOption()
    {
        var randomNum = Random.Range(0f, 1f);
        var total = randomOptions.Select(s => s.chanceWeighted).Sum();
        float startRange = 0;
        foreach (var option in randomOptions)
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


    public void GeneratorRandomOption(int level)
    {
        var option = getRandomOption();

        option.GenerateRoom(level, transform);
    }

    public void RemoveTilesWithinCollider(Collider2D collider)
    {
        var tilemap = GameObject.Find("Shadow").GetComponent<Tilemap>();
        
        Bounds bounds = collider.bounds;

        for (float x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (float y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0);
                Vector3 worldPos = tilemap.CellToWorld(tilePos);
                if (Physics2D.OverlapArea(worldPos - tilemap.cellSize / 2f, worldPos + tilemap.cellSize / 2f))
                {
                    tilemap.SetTile(tilePos, null);
                }
            }
        }

    }
}
