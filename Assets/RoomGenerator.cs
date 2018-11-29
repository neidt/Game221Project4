using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;

    public int roomCount = 0;
    public int MAXROOMS = 15;
    //public int levelWidth = 5;
    //public int levelHeight = 5;

    public Vector3 levelTransform;
    public Quaternion levelRotation;

    public List<Room> rooms = new List<Room>();
    public List<Room> branchRooms = new List<Room>();

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
        if (roomCount <= MAXROOMS)
        {
            //check for up, down, left, and right
            if (roomStart.isOpenRight)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x + 1, roomStart.roomPos.y);

                rooms.Add(createdRoom);
                branchRooms.Add(createdRoom);

                createdRoom.roomPos.x = roomStart.roomPos.x + 1;
                createdRoom.roomPos.y = roomStart.roomPos.y;
                createdRoom.PrintRoomInfo();
                //Debug.Log("room at: " + createdRoom.roomPos.ToString());

                // Create a connection from the roomStart parameter to the newly created room
                Connection connectionFromRoom = new Connection(Vector2Int.right, createdRoom);
                roomStart.connections.Add(connectionFromRoom);
                roomStart.isOpenRight = true;

                // TODO: Repeat this process for the connection from createdRoom to roomStart
                Connection connectionToRoom = new Connection(Vector2Int.left, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenLeft = true;

                //print out connection info for both ways
                connectionFromRoom.PrintConnectionInfo();
                connectionToRoom.PrintConnectionInfo();
            }
            if (roomStart.isOpenLeft)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x - 1, roomStart.roomPos.y);

                rooms.Add(createdRoom);
                branchRooms.Add(createdRoom);

                createdRoom.roomPos.x = roomStart.roomPos.x - 1;
                createdRoom.roomPos.y = roomStart.roomPos.y;
                createdRoom.PrintRoomInfo();
                //Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connectionFromRoom = new Connection(Vector2Int.left, createdRoom);
                roomStart.connections.Add(connectionFromRoom);
                roomStart.isOpenLeft = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.right, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenRight = true;

                //print out connection info for both ways
                connectionFromRoom.PrintConnectionInfo();
                connectionToRoom.PrintConnectionInfo();
            }
            if (roomStart.isOpenUp)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y + 1);

                rooms.Add(createdRoom);
                branchRooms.Add(createdRoom);

                createdRoom.roomPos.x = roomStart.roomPos.x;
                createdRoom.roomPos.y = roomStart.roomPos.y + 1;
                createdRoom.PrintRoomInfo();
                //Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connectionFromRoom = new Connection(Vector2Int.up, createdRoom);
                roomStart.connections.Add(connectionFromRoom);
                roomStart.isOpenUp = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.down, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenDown = true;

                //print out connection info for both ways
                connectionFromRoom.PrintConnectionInfo();
                connectionToRoom.PrintConnectionInfo();
            }
            if (roomStart.isOpenDown)
            {
                Room createdRoom = CreateRoom(roomStart.roomPos.x, roomStart.roomPos.y - 1);

                rooms.Add(createdRoom);
                branchRooms.Add(createdRoom);

                createdRoom.roomPos.x = roomStart.roomPos.x;
                createdRoom.roomPos.y = roomStart.roomPos.y - 1;
                createdRoom.PrintRoomInfo();
                //Debug.Log("room at: " + createdRoom.roomPos.ToString());

                //make connection
                Connection connectionFromRoom = new Connection(Vector2Int.down, createdRoom);
                roomStart.connections.Add(connectionFromRoom);
                roomStart.isOpenDown = true;

                //open the opposite sides's wall
                Connection connectionToRoom = new Connection(Vector2Int.up, roomStart);
                createdRoom.connections.Add(connectionToRoom);
                createdRoom.isOpenUp = true;

                //print out connection info for both ways
                connectionFromRoom.PrintConnectionInfo();
                connectionToRoom.PrintConnectionInfo();
            }
        }

        //if there is still an open spot, run this again,
        //but use a different room as the starting point
        if (roomCount <= MAXROOMS)
        {
            Room nextRoomStart = rooms[Random.Range(0, rooms.Count-1)];
            //Room nextRoomStart = rooms[0];

            //get a random closed direction and open it up
            //some of them will already be open, so dont use that one
            //get a room with a closed wall 

            //while loop way:
            //while room that is selcted is open on 4 sides, choose a new room
            //once room is chosen, select one of its closed sides to open up
            //then: you have to avoid stacking rooms on top of each other
            //make a list of rooms already created, so you dont make them twice
            //then check if the new room already exists, then get rid of it if it does
            while (nextRoomStart.isOpenUp && nextRoomStart.isOpenDown &&
                nextRoomStart.isOpenRight && nextRoomStart.isOpenLeft)
            {
                //pick a random room to be the start point
                //nextRoomStart = branchRooms[1];
                nextRoomStart = branchRooms[Random.Range(0, branchRooms.Count-1)];

                //pick a random side to open
                ChooseRandomSideToOpen(nextRoomStart);

                //if the room already exists, don't put anything else in this spot
                CreateRooms(nextRoomStart);
                

                //if (branchRooms.Contains(nextRoomStart))
                //{
                //    rooms.Remove(nextRoomStart);
                //    Destroy(nextRoomStart.roomGameObj);
                //}

                //MakeWalls(nextRoomStart);
            }
            //CreateRooms(nextRoomStart);
        }

    }//end create rooms

    //picks a random side to open that isn't already open
    private void ChooseRandomSideToOpen(Room roomToOpen)
    {
        int side = Random.Range(0, 4);
        if (side == 0 && !roomToOpen.isOpenUp)
        {
            print("chose up to open on room: " + roomToOpen.roomPos.ToString());
            roomToOpen.isOpenUp = true;
            roomToOpen.FrontWall.SetActive(false);

            //Connection newConnectionDown = new Connection(Vector2Int.down, roomToOpen);
            //roomToOpen.connections.Add(newConnectionDown);
        }
        if (side == 1 && !roomToOpen.isOpenRight)
        {
            print("chose right to open on room: " + roomToOpen.roomPos.ToString());
            roomToOpen.isOpenRight = true;
            roomToOpen.RightWall.SetActive(false);

            //Connection newConnectionLeft = new Connection(Vector2Int.left, roomToOpen);
            //roomToOpen.connections.Add(newConnectionLeft);
        }
        if (side == 2 && !roomToOpen.isOpenDown)
        {
            print("chose down to open on room: " + roomToOpen.roomPos.ToString());
            roomToOpen.isOpenDown = true;
            roomToOpen.BackWall.SetActive(false);

            //Connection newConnectionUp = new Connection(Vector2Int.up, roomToOpen);
            //roomToOpen.connections.Add(newConnectionUp);
        }
        if (side == 3 && !roomToOpen.isOpenLeft)
        {
            print("chose left to open on room: " + roomToOpen.roomPos.ToString());
            roomToOpen.isOpenLeft = true;
            roomToOpen.LeftWall.SetActive(false);

            //Connection newConnectionRight = new Connection(Vector2Int.right, roomToOpen);
            //roomToOpen.connections.Add(newConnectionRight);
        }
    }//end choose side

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
    }//end create room

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

    public void PrintRoomInfo()
    {
        if (isOpenUp)
        {
            Debug.Log("Room is open up: " + roomPos.ToString());
        }
        if (isOpenRight)
        {
            Debug.Log("Room is open right: " + roomPos.ToString());
        }
        if (isOpenDown)
        {
            Debug.Log("Room is open down: " + roomPos.ToString());
        }
        if (isOpenLeft)
        {
            Debug.Log("Room is open left: " + roomPos.ToString());
        }
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

    public void PrintConnectionInfo()
    {
        if (directionCameFrom.Equals(Vector2Int.up))
        {
            Debug.Log("Direction Connected: Up " + " Room Connected to: " + roomConnectedTo.roomPos.ToString());
        }
        if (directionCameFrom.Equals(Vector2Int.right))
        {
            Debug.Log("Direction Connected: Right " + " Room Connected to: " + roomConnectedTo.roomPos.ToString());
        }
        if (directionCameFrom.Equals(Vector2Int.down))
        {
            Debug.Log("Direction Connected: Down " + " Room Connected to: " + roomConnectedTo.roomPos.ToString());
        }
        if (directionCameFrom.Equals(Vector2Int.left))
        {
            Debug.Log("Direction Connected: Left " + " Room Connected to: " + roomConnectedTo.roomPos.ToString());
        }
    }
}
