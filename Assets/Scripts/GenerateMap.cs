using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Linq;
using System;

public class GenerateMap : MonoBehaviour
{
    public Room spawnRoom;
    public Room finishRoom;
    public UnityEngine.Object[] rooms;
    public int roomCount = 0;
    public int roomsToGenerate = 4;
    public List<Room> generatedRooms = new List<Room>();
    // Start is called before the first frame update
    void Start()
    {
        rooms = Resources.LoadAll("Rooms", typeof(Room));
        List<Room> rooms2 = new List<Room>();
        for(int i = 0; i < rooms.Length; i++)
        {
            Debug.Log(rooms[i].name);
        }
        for(int i = 0; i < rooms.Length; i++)
        {
            rooms2.Add((Room)rooms[i]);
        }

        Room newRoom = Instantiate(spawnRoom, new Vector3(0, 0, 0), Quaternion.identity);
        Room lastRoom = newRoom;

        //create a new room, using the first room in the array, adjacent to the spawn room
        for (int i = 0; i < roomsToGenerate; i++)
        {
            Debug.Log("Name: " + lastRoom.transform.name + "Position: " + lastRoom.transform.position.x + "width: " + lastRoom.width);

            String doorSide;
            if(lastRoom.leftSocket < 0)
            {
                doorSide = "left";
                List<Room> validRooms = rooms2.Where(room => room.rightSocket == Math.Abs(lastRoom.leftSocket)).ToList();
                Room currentRoom = (Room)validRooms[UnityEngine.Random.Range(0, validRooms.Count)];
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x - lastRoom.width - ((currentRoom.width / 2) + (lastRoom.width / 2)), lastRoom.transform.position.y - currentRoom.inDoor, 0), Quaternion.identity);
            }
            else if(lastRoom.rightSocket < 0)
            {
                doorSide = "right";
                List<Room> validRooms = rooms2.Where(room => room.leftSocket == Math.Abs(lastRoom.rightSocket)).ToList();
                Room currentRoom = (Room)validRooms[UnityEngine.Random.Range(0, validRooms.Count)]; ;
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x + lastRoom.width + ((currentRoom.width / 2) - (lastRoom.width / 2)), lastRoom.transform.position.y - currentRoom.inDoor, 0), Quaternion.identity);
            }
            else if(lastRoom.topSocket < 0)
            {
                doorSide = "top";
                List<Room> validRooms = rooms2.Where(room => room.bottomSocket == Math.Abs(lastRoom.topSocket)).ToList();
                Room currentRoom = (Room)validRooms[UnityEngine.Random.Range(0, validRooms.Count)];
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x - currentRoom.inDoor, lastRoom.transform.position.y + lastRoom.height, 0), Quaternion.identity);
            }
            else if( lastRoom.bottomSocket < 0)
            {
                doorSide = "bottom";
                List<Room> validRooms = rooms2.Where(room => room.topSocket == Math.Abs(lastRoom.bottomSocket)).ToList();
                Room currentRoom = (Room)validRooms[UnityEngine.Random.Range(0, validRooms.Count)];
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x - currentRoom.inDoor, lastRoom.transform.position.y - lastRoom.height, 0), Quaternion.identity);
            }
            else 
            {
                Debug.Log("No door found");
                break;
            }

            

            
            lastRoom = newRoom;
            generatedRooms.Add(newRoom);
            roomCount++;
        }

        newRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x + (lastRoom.width / 2) + (finishRoom.width / 2), lastRoom.transform.position.y, 0), Quaternion.identity);
    }
}
