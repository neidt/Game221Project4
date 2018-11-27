using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 0;
    public const int MAXROOMS = 8;
    public int levelWidth = 5;
    public int levelHeight = 5;

    public Vector3 levelTransform;
    public Quaternion levelRotation;

    public List<Room> rooms = new List<Room>();

    // Use this for initialization
    void Start()
    {
        if (roomCount == 0)
        {
            Room roomStart = CreateRoom(0, 0);
            roomCount++;
            if (roomCount < MAXROOMS)
            {
                CreateRooms(roomStart);

                //check if the room is in a valid place
                //CheckIfValid(newRoom);

            }
        }
    }//end start

    // Update is called once per frame
    void Update()
    {

    }//end update

    public void CreateRooms(Room roomStart)
    {
        //check for up, down, left, and right
        if (roomStart.isOpenRight)
        {
            Room createdRoom = CreateRoom(roomStart.roomPos.x + 1, roomStart.roomPos.y);
            Connection connection = new Connection(Vector2Int.right, createdRoom);
            roomStart.connections.Add(connection);
        }
        if (roomStart.isOpenLeft)
        {
            Room createdRoom = CreateRoom(roomStart.roomPos.x - 1, roomStart.roomPos.y);
            Connection connection = new Connection(Vector2Int.left, createdRoom);
            roomStart.connections.Add(connection);
        }
        if (roomStart.isOpenUp)
        {
            roomStart.isOpenUp = true;
            CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y + 1);
        }
        if (roomStart.isOpenDown)
        {
            roomStart.isOpenDown = true;
            CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y - 1);
        }

    }//end create rooms

    private Room CreateRoom(int x, int y)
    {
        GameObject room = GameObject.Instantiate(roomPrefab);

        Vector3 transformPos = levelTransform;
        transformPos.x += x * roomPrefab.transform.localScale.x;
        transformPos.z += y * roomPrefab.transform.localScale.y;
        room.transform.position = transformPos;
        Room roomRoom = new Room(room.transform.position);
        return roomRoom;
    }

    private void MakeWalls()
    {
        //spawn walls based on room position

    }//end make walls


}

public class Room
{
    public bool isOpenRight;
    public bool isOpenLeft;
    public bool isOpenUp;
    public bool isOpenDown;

    public Vector2Int roomPos;
    public List<Connection> connections = new List<Connection>();

    public Room(Vector3 pos)
    {
        isOpenRight = true;
        isOpenLeft = true;
        isOpenUp = true;
        isOpenDown = true;
    }
}

public class Connection
{
    public Vector2Int directionCameFrom;
    public Room roomConnectedTo;

    public Connection(Vector2Int directionConnected, Room connectedRoom)
    {
        directionCameFrom = directionConnected;
        roomConnectedTo = connectedRoom;
    }
}
