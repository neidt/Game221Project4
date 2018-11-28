using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    //public GameObject FrontWall, BackWall, RightWall, LeftWall;

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
            rooms.Add(roomStart);
            //roomCount++;

            roomStart.isOpenUp = true;
            roomStart.isOpenDown = true;
            roomStart.isOpenRight = true;
            roomStart.isOpenLeft = true;

            CreateRooms(roomStart);


            //make the walls (or unmake them in this case)
            foreach (Room room in rooms)
            {
                MakeWalls(room);
            }
        }
    }//end start

    // Update is called once per frame
    void Update()
    {
    }//end update

    public void CreateRooms(Room roomStart)
    {
        if (roomCount < MAXROOMS)
        {
            //check for up, down, left, and right
            if (roomStart.isOpenRight)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x + 1, roomStart.roomPos.y);
                rooms.Add(createdRoom);
                createdRoom.roomPos.x = roomStart.roomPos.x + 1;
                createdRoom.roomPos.y = roomStart.roomPos.y;
                Debug.Log("room at X: " + createdRoom.roomPos.ToString());

                // Create a connection from the roomStart parameter to the newly created room
                Connection connection = new Connection(Vector2Int.right, createdRoom);
                roomStart.connections.Add(connection);
                roomStart.isOpenRight = true;

                // TODO: Repeat this process for the connection from createdRoom to roomStart
                Connection connectionToRoom = new Connection(Vector2Int.left, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenLeft = true;
            }
            if (roomStart.isOpenLeft)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x - 1, roomStart.roomPos.y);
                rooms.Add(createdRoom);
                createdRoom.roomPos.x = roomStart.roomPos.x - 1;
                createdRoom.roomPos.y = roomStart.roomPos.y;
                Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connection = new Connection(Vector2Int.left, createdRoom);
                roomStart.connections.Add(connection);
                roomStart.isOpenLeft = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.right, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenRight = true;
            }
            if (roomStart.isOpenUp)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y + 1);
                rooms.Add(createdRoom);
                createdRoom.roomPos.x = roomStart.roomPos.x;
                createdRoom.roomPos.y = roomStart.roomPos.y + 1;
                Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connection = new Connection(Vector2Int.up, createdRoom);
                roomStart.connections.Add(connection);
                roomStart.isOpenUp = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.down, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenDown = true;
            }
            if (roomStart.isOpenDown)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y - 1);
                rooms.Add(createdRoom);
                createdRoom.roomPos.x = roomStart.roomPos.x;
                createdRoom.roomPos.y = roomStart.roomPos.y - 1;
                Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connectionFromRoom = new Connection(Vector2Int.down, createdRoom);
                roomStart.connections.Add(connectionFromRoom);
                roomStart.isOpenDown = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.up, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenUp = true;
            }

            //if there is still an open spot, run this again,
            //but use a different room as the starting point
            if (roomCount < MAXROOMS)
            {
                Room nextRoomStart = rooms[Random.Range(0, rooms.Count)];
                //get a random closed direction and open it up
                //some of them will already be open, so dont use that one
                //get a room with a closed wall 

                //while loop way:
                //while room that is selcted is open on 4 sides, choose a new room
                //once room is chosen, select one of its closed sides to open up
                //then: you have to avoid stacking rooms on top of each other
                //make a list of rooms already created, so you dont make them twice
                //then check if the new room already exists, then get rid of it if it does
                while(nextRoomStart.isOpenUp && nextRoomStart.isOpenDown &&
                    nextRoomStart.isOpenRight && nextRoomStart.isOpenLeft)
                {

                    CreateRooms(nextRoomStart);
                } 
                //CreateRooms(nextRoomStart);
            }
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
        roomRoom.SetGameObject(room);
        roomCount++;
        return roomRoom;
    }

    private void MakeWalls(Room room)
    {
        Room roomToWall = room;
        //spawn walls based on room position
        //if room is open front
        if (roomToWall.isOpenUp)
        {
            roomToWall.FrontWall.SetActive(false);
        }
        //if room is open down
        if (roomToWall.isOpenDown)
        {
            roomToWall.BackWall.SetActive(false);
        }
        //if room is open left
        if (roomToWall.isOpenLeft)
        {
            roomToWall.LeftWall.SetActive(false);
        }
        //if room is open right
        if (roomToWall.isOpenRight)
        {
            roomToWall.RightWall.SetActive(false);
        }
    }//end make walls


}

public class Room
{
    public bool isOpenRight;
    public bool isOpenLeft;
    public bool isOpenUp;
    public bool isOpenDown;

    public GameObject roomGameObj;
    public GameObject FrontWall, BackWall, RightWall, LeftWall;

    public Vector2Int roomPos;
    public List<Connection> connections = new List<Connection>();

    public Room(Vector3 pos)
    {
        isOpenRight = false;
        isOpenLeft = false;
        isOpenUp = false;
        isOpenDown = false;

    }

    public void SetGameObject(GameObject obj)
    {
        roomGameObj = obj;
        this.FrontWall = obj.transform.Find("FrontWall").gameObject;
        this.BackWall = obj.transform.Find("BackWall").gameObject;
        this.RightWall = obj.transform.Find("RightWall").gameObject;
        this.LeftWall = obj.transform.Find("LeftWall").gameObject;
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
