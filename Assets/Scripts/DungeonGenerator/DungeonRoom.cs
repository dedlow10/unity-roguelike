using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DungeonRoom
{
    public string id;
    public Vector2Int size;
    public Vector2Int topLeft;
    public Vector2Int topRight;
    public Vector2Int bottomLeft;
    public Vector2Int bottomRight;

    public DungeonRoom roomLeft;
    public DungeonRoom roomTop;
    public DungeonRoom roomBottom;
    public DungeonRoom roomRight;

    public DungeonRoom(Vector2Int bottomLeft, Vector2Int size)
    {
        this.size = size;
        this.bottomLeft = bottomLeft;
        this.bottomRight = new Vector2Int(bottomLeft.x + size.x - 1, bottomLeft.y);
        this.topLeft = new Vector2Int(bottomLeft.x, bottomLeft.y + size.y - 1);
        this.topRight = new Vector2Int(bottomRight.x, topLeft.y);
        this.id = new Guid().ToString();
    }

    public Vector2Int getCenter()
    {
        var midX = topLeft.x + (size.x / 2);
        var midY = topLeft.y - (size.y / 2) + 1;
        return new Vector2Int(midX, midY);
    }

    public bool IsStartRoom { get; set; }
    public bool IsBossRoom { get; set; }

    public static DungeonRoom FindRoomAtCoordinates(List<DungeonRoom> rooms, Vector2Int bottomLeft)
    {
        var foundRoom = rooms.Where(r => r.bottomLeft.Equals(bottomLeft)).FirstOrDefault();
        return foundRoom;
    }
}