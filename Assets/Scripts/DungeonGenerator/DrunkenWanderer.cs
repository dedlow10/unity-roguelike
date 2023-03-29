using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrunkenWanderer
{
    int iterations;
    int iterationLength;
    Vector2Int roomSize;
    int corridorLength;
    List<DungeonRoom> existingRooms = new();

    public DrunkenWanderer(int iterations, int iterationLength, Vector2Int roomSize, int corridorLength)
    {
        this.iterations = iterations;
        this.iterationLength = iterationLength;
        this.roomSize = roomSize;
        this.corridorLength = corridorLength;
    }

    public List<DungeonRoom> GenerateRooms(Vector2Int vector2Start)
    {
        var room = new DungeonRoom(vector2Start, roomSize);
        existingRooms.Add(room);

        for (var x = 0; x < iterations; x++)
        {
            Wander(room);
        }

      foreach(var test in existingRooms)
    {
            Debug.Log(test.getCenter());
            Debug.Log("Left: " + (test.roomLeft != null).ToString());
            Debug.Log("Right: " + (test.roomRight != null).ToString());
            Debug.Log("Top: " + (test.roomTop != null).ToString());
            Debug.Log("Bottom: " + (test.roomBottom != null).ToString());
    }

        // fix rooms
        /*
        foreach(var currentRoom in existingRooms)
        {
            if (currentRoom.roomTop != null)
            {
                var foundRoom = existingRooms.Where(e => e.id == currentRoom.roomTop.id).FirstOrDefault();
                if (foundRoom.roomBottom == null)
                {
                    foundRoom.roomBottom = currentRoom;
                }
                else
                {
                    currentRoom.roomTop = null;
                }
            }

            if (currentRoom.roomBottom != null)
            {
                var foundRoom = existingRooms.Where(e => e.id == currentRoom.roomBottom.id).FirstOrDefault();
                if (foundRoom.roomTop == null)
                {
                    foundRoom.roomTop = currentRoom;
                }
                else
                {
                    currentRoom.roomBottom = null;
                }
            }

            if (currentRoom.roomLeft != null)
            {
                var foundRoom = existingRooms.Where(e => e.id == currentRoom.roomLeft.id).FirstOrDefault();
                if (foundRoom.roomRight == null)
                {
                    foundRoom.roomRight = currentRoom;
                }
                else
                {
                    currentRoom.roomLeft = null;
                }
            }

            if (currentRoom.roomRight != null)
            {
                var foundRoom = existingRooms.Where(e => e.id == currentRoom.roomRight.id).FirstOrDefault();
                if (foundRoom.roomLeft == null)
                {
                    foundRoom.roomLeft = currentRoom;
                }
                else
                {
                    currentRoom.roomTop = null;
                }
            }
        }  
        */

        return existingRooms;
    }

    private List<DungeonRoom> Wander(DungeonRoom roomStart)
    {
        List<DungeonRoom> rooms = new();
        for (var x = 0; x < iterationLength; x++)
        {
            DungeonRoom newRoom = null;
            var num = Random.Range(1, 4);
            if (num == 1)
            {
                var foundRoom = DungeonRoom.FindRoomAtCoordinates(existingRooms, new Vector2Int(roomStart.topLeft.x, roomStart.topLeft.y + corridorLength));
                if (foundRoom == null)
                {
                    newRoom = GenerateRoomTop(roomStart);
                    existingRooms.Add(newRoom);
                }
                else
                {
                    roomStart.roomTop = foundRoom;
                    foundRoom.roomBottom = roomStart;
                    newRoom = foundRoom;
                }
            }
            else if (num == 2)
            {
                var foundRoom = DungeonRoom.FindRoomAtCoordinates(existingRooms, new Vector2Int(roomStart.topLeft.x, roomStart.bottomLeft.y - corridorLength - roomSize.y + 1));
                if (foundRoom == null)
                {
                    newRoom = GenerateRoomBottom(roomStart);
                    existingRooms.Add(newRoom);
                }
                else
                {
                    roomStart.roomBottom = foundRoom;
                    foundRoom.roomTop = roomStart;
                    newRoom = foundRoom;
                }
            }
            else if (num == 3)
            {
                var foundRoom = DungeonRoom.FindRoomAtCoordinates(existingRooms, new Vector2Int(roomStart.bottomLeft.x - corridorLength - roomSize.x + 1, roomStart.bottomLeft.y));
                if (foundRoom == null)
                {
                    newRoom = GenerateRoomLeft(roomStart);
                    existingRooms.Add(newRoom);
                }
                else
                {
                    roomStart.roomLeft = foundRoom;
                    foundRoom.roomRight = roomStart;
                    newRoom = foundRoom;
                }
            }
            else if (num == 4)
            {
                var foundRoom = DungeonRoom.FindRoomAtCoordinates(existingRooms, new Vector2Int(roomStart.bottomRight.x + corridorLength, roomStart.bottomRight.y));
                if (foundRoom == null)
                {
                    newRoom = GenerateRoomRight(roomStart);
                    existingRooms.Add(newRoom);
                }
                else
                {
                    roomStart.roomRight = foundRoom;
                    foundRoom.roomLeft = roomStart;
                    newRoom = foundRoom;
                }
            }

            roomStart = newRoom;
        }

        return rooms;
  
    }

    private DungeonRoom GenerateRoomTop(DungeonRoom room)
    {
        var roomTop = new DungeonRoom(new Vector2Int(room.topLeft.x, room.topLeft.y + corridorLength), roomSize);
        roomTop.roomBottom = room;
        room.roomTop = roomTop;

        return roomTop;
    }
    private DungeonRoom GenerateRoomBottom(DungeonRoom room)
    {
        var roomBottom = new DungeonRoom(new Vector2Int(room.topLeft.x, room.bottomLeft.y - corridorLength - roomSize.y + 1), roomSize);
        roomBottom.roomTop = room;
        room.roomBottom = roomBottom;

        return roomBottom;
    }
    private DungeonRoom GenerateRoomRight(DungeonRoom room)
    {
        var roomRight = new DungeonRoom(new Vector2Int(room.bottomRight.x + corridorLength, room.bottomRight.y), roomSize);
        roomRight.roomLeft = room;
        room.roomRight = roomRight;

        return roomRight;
    }
    private DungeonRoom GenerateRoomLeft(DungeonRoom room)
    {
        var roomLeft = new DungeonRoom(new Vector2Int(room.bottomLeft.x - corridorLength - roomSize.x + 1, room.bottomLeft.y), roomSize);
        roomLeft.roomRight = room;
        room.roomLeft = roomLeft;

        return roomLeft;
    }
}
