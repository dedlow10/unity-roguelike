using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap minimapTilemap;
    [SerializeField] TileBase floorTile;
    [SerializeField] TileBase corridorTile;
    [SerializeField] TileBase minimapFloorTile;
    [SerializeField] int CorridorLength;
    [SerializeField] GameObject doorVertical;
    [SerializeField] GameObject doorHorizontal;
    [SerializeField] int NumberOfIterations;
    [SerializeField] int IterationLength;

    [SerializeField] TileBase WallLeft;
    [SerializeField] TileBase WallRight;
    [SerializeField] TileBase WallTop;
    [SerializeField] TileBase WallBottom;
    [SerializeField] TileBase WallTopLeft;
    [SerializeField] TileBase WallTopRight;
    [SerializeField] TileBase WallBottomLeft;
    [SerializeField] TileBase WallBottomRight;

    [SerializeField] GameObject RoomPrefab;

    [SerializeField] EnemyRoom[] EnemyRooms;
    [SerializeField] ChestRoom[] ChestRooms;
    [SerializeField] EnemyRoom[] BossRooms;

    Vector2Int[] roomSizes = new Vector2Int[] { new Vector2Int { x = 12, y = 8 } };

    private void Start()
    {
        GenerateDungeon();
        AstarPath.active.Scan();
    }

    void GenerateDungeon()
    {
        var rooms = GenerateRooms();
        var endRooms = GetAllEndRooms(rooms);

        GenerateFloors(rooms);
        GenerateWalls(rooms);
        GenerateCorridors(rooms);

        var playerTransform = GameManager.instance.GetPlayer().GetComponent<Transform>();
        var startRoom = endRooms[0];
        var bossRoom = endRooms[1];

        startRoom.IsStartRoom = true;
        bossRoom.IsBossRoom = true;
        playerTransform.position = new Vector3(startRoom.topLeft.x + 2, startRoom.bottomLeft.y + 2, 0);


        GenerateRoomControllers(rooms);
    }

    private List<DungeonRoom> GetAllEndRooms(List<DungeonRoom> allRooms)
    {
        return allRooms.Where(ar => new List<DungeonRoom> { ar.roomTop, ar.roomBottom, ar.roomLeft, ar.roomRight }.Where(sr => sr != null).Count() == 1).ToList();
    }

    public List<DungeonRoom> GenerateRooms()
    {
        var size = roomSizes[0];

        
        var bounds = groundTilemap.cellBounds;
        Vector3 startPos = bounds.center;
        var vector2Start = new Vector2Int(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y));

        var wanderer = new DrunkenWanderer(NumberOfIterations, IterationLength, size, CorridorLength);
        return wanderer.GenerateRooms(vector2Start);
    }

    public void GenerateCorridors(List<DungeonRoom> rooms)
    {
        foreach(var room in rooms)
        {
            if (room.roomTop != null)
            {
                var doorPositionX = room.topRight.x - (room.size.x / 2) + 1;

                for (var y = 0; y <= CorridorLength; y++)
                {
                    var tile = y == 0 || y == CorridorLength ? floorTile : corridorTile;

                    groundTilemap.SetTile(new Vector3Int(doorPositionX, room.topLeft.y + y, 0), tile);
                    groundTilemap.SetTile(new Vector3Int(doorPositionX - 1, room.topLeft.y + y, 0), tile);
                    groundTilemap.RefreshTile(new Vector3Int(doorPositionX, room.topLeft.y + y, 0));
                    groundTilemap.RefreshTile(new Vector3Int(doorPositionX - 1, room.topLeft.y + y, 0));

                    if (minimapTilemap != null)
                    {
                        minimapTilemap.SetTile(new Vector3Int(doorPositionX, room.topLeft.y + y, 0), minimapFloorTile);
                        minimapTilemap.SetTile(new Vector3Int(doorPositionX - 1, room.topLeft.y + y, 0), minimapFloorTile);
                        minimapTilemap.RefreshTile(new Vector3Int(doorPositionX, room.topLeft.y + y, 0));
                        minimapTilemap.RefreshTile(new Vector3Int(doorPositionX - 1, room.topLeft.y + y, 0));
                    }

                    if (y > 0 && y < CorridorLength)
                    {
                        wallTilemap.SetTile(new Vector3Int(doorPositionX + 1, room.topLeft.y + y, 0), WallLeft);
                        wallTilemap.SetTile(new Vector3Int(doorPositionX - 2, room.topLeft.y + y, 0), WallRight);
                        wallTilemap.RefreshTile(new Vector3Int(doorPositionX + 1, room.topLeft.y + y, 0));
                        wallTilemap.RefreshTile(new Vector3Int(doorPositionX - 2, room.topLeft.y + y, 0));
                    }
                }
            }

            if (room.roomRight != null)
            {
                var doorPositionY = room.topRight.y - (room.size.y / 2) + 1;

                for (var x = 0; x <= CorridorLength; x++)
                {
                    var tile = x == 0 || x == CorridorLength ? floorTile : corridorTile;

                    groundTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY, 0), tile);
                    groundTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY - 1, 0), tile);
                    groundTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY, 0));
                    groundTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY - 1, 0));



                    if (minimapTilemap != null)
                    {
                        minimapTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY, 0), minimapFloorTile);
                        minimapTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY - 1, 0), minimapFloorTile);
                        minimapTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY, 0));
                        minimapTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY - 1, 0));
                    }

                    if (x > 0 && x < CorridorLength)
                    {
                        wallTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY + 1, 0), WallBottom);
                        wallTilemap.SetTile(new Vector3Int(room.topRight.x + x, doorPositionY - 2, 0), WallTop);
                        wallTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY + 1, 0));
                        wallTilemap.RefreshTile(new Vector3Int(room.topRight.x + x, doorPositionY -2 , 0));
                    }
                }
            }
        }
    }

    public void GenerateFloors(List<DungeonRoom> rooms)
    {
        foreach(var room in rooms)
        {
            for (var x = 0; x < room.size.x; x++)
            {
                for (var y = 0; y < room.size.y; y++)
                {
                    if (y == 0 || y + 1 == room.size.y)
                    {
                        continue;
                    }
                    groundTilemap.SetTile(new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y + y, 0), floorTile);
                    groundTilemap.RefreshTile(new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y + y, 0));

                    if (minimapTilemap != null)
                    {
                        minimapTilemap.SetTile(new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y + y, 0), minimapFloorTile);
                        minimapTilemap.RefreshTile(new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y + y, 0));
                    }
                }
            }
        }
    }

    public void GenerateWalls(List<DungeonRoom> rooms)
    {
        foreach(var room in rooms)
        {

            // Corner Tiles
            wallTilemap.SetTile(new Vector3Int(room.topLeft.x, room.topLeft.y, 0), WallTopLeft);
            wallTilemap.RefreshTile(new Vector3Int(room.topLeft.x, room.topLeft.y, 0));

            wallTilemap.SetTile(new Vector3Int(room.topRight.x, room.topRight.y, 0), WallTopRight);
            wallTilemap.RefreshTile(new Vector3Int(room.topRight.x, room.topRight.y, 0));

            wallTilemap.SetTile(new Vector3Int(room.bottomLeft.x, room.bottomLeft.y, 0), WallBottomLeft);
            wallTilemap.RefreshTile(new Vector3Int(room.bottomLeft.x, room.bottomLeft.y, 0));

            wallTilemap.SetTile(new Vector3Int(room.bottomRight.x, room.bottomRight.y, 0), WallBottomRight);
            wallTilemap.RefreshTile(new Vector3Int(room.bottomRight.x, room.bottomRight.y, 0));

            //Side Tiles
            var doorTop = room.roomTop != null ? BuildDoor(room, "top") : null;
            var doorBottom = room.roomBottom != null ? BuildDoor(room, "bottom") : null;
            var doorLeft = room.roomLeft != null ? BuildDoor(room, "left") : null;
            var doorRight = room.roomRight != null ?  BuildDoor(room, "right") : null;
       
            for (var x = 1; x < room.size.x - 1; x++)
            {
                if (doorTop.HasValue && (room.topLeft.x + x == doorTop.Value.x || room.topLeft.x + x == doorTop.Value.x - 1))
                {
                    continue;
                }

                wallTilemap.SetTile(new Vector3Int(room.topLeft.x + x, room.topLeft.y, 0), WallTop);
                wallTilemap.RefreshTile((new Vector3Int(room.topLeft.x + x, room.topLeft.y, 0)));
            }
          
            for (var x = 1; x < room.size.x - 1; x++)
            {
                if (doorBottom.HasValue && (room.bottomLeft.x + x == doorBottom.Value.x || room.bottomLeft.x + x == doorBottom.Value.x - 1))
                {
                    continue;
                }
                wallTilemap.SetTile(new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y, 0), WallBottom);
                wallTilemap.RefreshTile((new Vector3Int(room.bottomLeft.x + x, room.bottomLeft.y, 0)));
            }

            for (var y = 1; y < room.size.y - 1; y++)
            {
                if (doorLeft.HasValue && (room.bottomLeft.y + y == doorLeft.Value.y || room.bottomLeft.y + y == doorLeft.Value.y - 1))
                {
                    continue;
                }
                wallTilemap.SetTile(new Vector3Int(room.bottomLeft.x, room.bottomLeft.y + y, 0), WallLeft);
                wallTilemap.RefreshTile((new Vector3Int(room.bottomLeft.x, room.bottomLeft.y + y, 0)));
            }
          

            for (var y = 1; y < room.size.y - 1; y++)
            {
                if (doorRight.HasValue && (room.bottomRight.y + y == doorRight.Value.y || room.bottomRight.y + y == doorRight.Value.y - 1))
                {
                    continue;
                }
                wallTilemap.SetTile(new Vector3Int(room.bottomRight.x, room.bottomRight.y + y, 0), WallRight);
                wallTilemap.RefreshTile((new Vector3Int(room.bottomRight.x, room.bottomRight.y + y, 0)));
            }
          
        }
    }

    public Vector3? BuildDoor(DungeonRoom room, string direction)
    {

        var doorsObj = GameObject.Find("Doors");
        if (direction == "top")
        {
            var doorPositionX = room.topRight.x - (room.size.x / 2) + 1;
            var vector3 = groundTilemap.CellToWorld(new Vector3Int(doorPositionX, room.topLeft.y, 0));
            vector3.y = vector3.y + .5f;
            Instantiate(doorHorizontal, vector3, Quaternion.identity, doorsObj.transform);

            return vector3;
        }
        else if (direction == "bottom")
        {
            var doorPositionX = room.topRight.x - (room.size.x / 2) + 1;
            var vector3 = groundTilemap.CellToWorld(new Vector3Int(doorPositionX, room.bottomLeft.y, 0));
            vector3.y = vector3.y + .5f;
            Instantiate(doorHorizontal, vector3, Quaternion.identity, doorsObj.transform);

            return vector3;
        }
        else if (direction == "left")
        {
            var doorPositionY = room.topRight.y - (room.size.y / 2) + 1;
            var vector3 = groundTilemap.CellToWorld(new Vector3Int(room.topLeft.x, doorPositionY, 0));
            //vector3.x = vector3.x + .8f;
            Instantiate(doorVertical, vector3, Quaternion.identity, doorsObj.transform);

            return vector3;
        }
        else if (direction == "right")
        {
            var doorPositionY = room.topRight.y - (room.size.y / 2) + 1;
            var vector3 = groundTilemap.CellToWorld(new Vector3Int(room.topRight.x, doorPositionY, 0));
            vector3.x = vector3.x + .75f;
            Instantiate(doorVertical, vector3, Quaternion.identity, doorsObj.transform);

            return vector3;
        }

        return null;
    }

    public void GenerateRoomControllers(List<DungeonRoom> rooms)
    {
        var roomContainer = GameObject.Find("Rooms");
        foreach (var room in rooms)
        {
            if (room.IsStartRoom) continue;

            var newRoom = Instantiate(RoomPrefab,  groundTilemap.CellToWorld(new Vector3Int(room.getCenter().x, room.getCenter().y, 0)), Quaternion.identity, roomContainer.transform);
            var roomController = newRoom.GetComponent<RoomController>();
            var boxCollider = newRoom.GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(room.size.x - .5f, room.size.y - .5f);
            if (room.IsBossRoom)
            {
                roomController.randomOptions = new RoomType[] { BossRooms[0] };
            }
            else
            {
                roomController.randomOptions = new RoomType[] { EnemyRooms[0], ChestRooms[0] };
            }

        }
    }
}
