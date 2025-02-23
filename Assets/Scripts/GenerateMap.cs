using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class GenerateMap : MonoBehaviour
{
    public Room spawnRoom;
    public Room finishRoom;
    public Object[] rooms;
    public int roomCount = 0;
    public int roomsToGenerate = 4;
    public List<Room> generatedRooms = new List<Room>();

    // Start is called before the first frame update
    void Start()
    {

        rooms = Resources.LoadAll("Rooms", typeof(Room));
        Room newRoom = Instantiate(spawnRoom, new Vector3(0, 0, 0), Quaternion.identity);
        Room lastRoom = newRoom;


        //create a new room, using the first room in the array, adjacent to the spawn room
        for (int i = 0; i < roomsToGenerate; i++)
        {
            Debug.Log("Position: " + lastRoom.transform.position.x + "width: " + lastRoom.width);
            Room currentRoom = (Room)rooms[Random.Range(0, rooms.Length)];
            newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x + lastRoom.width + ((currentRoom.width / 2) - (lastRoom.width / 2)), lastRoom.transform.position.y - currentRoom.inDoor, 0), Quaternion.identity);
            lastRoom = newRoom;
            generatedRooms.Add(newRoom);
            roomCount++;
        }

        newRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x + lastRoom.width, lastRoom.transform.position.y, 0), Quaternion.identity);
    }
}
