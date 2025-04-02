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
    public FinishRoom finishRoom;
    public CheckpointRoom checkpointRoom;
    private UnityEngine.Object[] rooms;
    public int roomCount = 0;
    public int roomsToGenerate = 4;
    private List<Room> generatedRooms = new List<Room>();


    // Start is called before the first frame update
    void Start()
    {
        //load all of the rooms from the resources folder
        rooms = Resources.LoadAll("Rooms", typeof(Room));
        List<Room> rooms2 = new List<Room>();

        //log all the rooms, and add them to rooms2 as type room
        for(int i = 0; i < rooms.Length; i++)
        {
            Debug.Log(rooms[i].name);
        }
        for(int i = 0; i < rooms.Length; i++)
        {
            rooms2.Add((Room)rooms[i]);
        }
        
        //create the spawn room, make it the last room
        FinishRoom finalRoom = null;
        CheckpointRoom checkpointRoomObj = null;
        Room newRoom = Instantiate(spawnRoom, new Vector3(0, 0, 0), Quaternion.identity);
        Room lastRoom = newRoom;

        //for the amount of rooms to generate, generate a new room. Every 5 rooms, spawn a checkpoint room. 
        for (int i = 0; i < roomsToGenerate; i++)
        {
            Debug.Log("Name: " + lastRoom.transform.name + "Position: " + lastRoom.transform.position.x + "width: " + lastRoom.width);

            if(i/5 == 1){
                int inDoor;
                int outDoor;
                Debug.Log("Spawning checkpoint room");
                if(lastRoom.rightSocket < 0)
                {
                    inDoor = 0;
                    if(lastRoom.leftSocket < 0){
                        outDoor = 3;
                    }
                    else{
                        outDoor = 1;
                    }
                    checkpointRoomObj = Instantiate(this.checkpointRoom, new Vector3(lastRoom.transform.position.x + (lastRoom.width / 2) + (checkpointRoom.width / 2), lastRoom.transform.position.y, 0), Quaternion.identity);
                    checkpointRoomObj.SetupRoom(inDoor, outDoor);
                }
                else if(lastRoom.leftSocket < 0)
                {
                    inDoor = 1;
                    if(lastRoom.rightSocket < 0){
                        outDoor = 3;
                    }
                    else{
                        outDoor = 0;
                    }
                    checkpointRoomObj = Instantiate(this.checkpointRoom, new Vector3(lastRoom.transform.position.x - (lastRoom.width / 2) - (checkpointRoom.width / 2), lastRoom.transform.position.y, 0), Quaternion.identity);
                    checkpointRoomObj.SetupRoom(inDoor, outDoor);
                }
                else if(lastRoom.topSocket < 0)
                {
                    inDoor = 3;
                    if(lastRoom.rightSocket < 0){
                        outDoor = 0;
                    }
                    else{
                        outDoor = 1;
                    }
                    checkpointRoomObj = Instantiate(this.checkpointRoom, new Vector3(lastRoom.transform.position.x, lastRoom.transform.position.y + (lastRoom.height / 2) + (checkpointRoom.height / 2)), Quaternion.identity);
                    checkpointRoomObj.SetupRoom(inDoor, outDoor);
                }
                else if(lastRoom.bottomSocket < 0)
                {
                    inDoor = 2;
                    if(lastRoom.rightSocket < 0){
                        outDoor = 0;
                    }
                    else{
                        outDoor = 1;
                    }
                    checkpointRoomObj = Instantiate(this.checkpointRoom, new Vector3(lastRoom.transform.position.x, lastRoom.transform.position.y - (lastRoom.height / 2) - (checkpointRoom.height / 2)), Quaternion.identity);
                    checkpointRoomObj.SetupRoom(inDoor, outDoor);
                }
                else
                {
                    Debug.Log("No door found");
                    break;
                }
                lastRoom = checkpointRoomObj;
                generatedRooms.Add(checkpointRoomObj);
            }

            //create and fill up the valid rooms list, based on the last room's door
            List<Room> validRooms = new List<Room>();

            if(lastRoom.leftSocket > 0)
            {
                validRooms = rooms2.Where(room => room.leftSocket >= 0).ToList();
            }
            else if(lastRoom.rightSocket > 0)
            {
                validRooms = rooms2.Where(room => room.rightSocket >= 0).ToList();
            }
            else if(lastRoom.topSocket > 0)
            {
                validRooms = rooms2.Where(room => room.topSocket >= 0).ToList();
            }
            else if(lastRoom.bottomSocket > 0)
            {
                validRooms = rooms2.Where(room => room.topSocket >= 0).ToList();
            }
            else
            {
                validRooms = rooms2;
            }

            Debug.Log("Valid Rooms: " + validRooms.Count);

            //update the valid rooms list to exclude anything with the same out door as the last room's in door. Then, spawn the room. 
            if(lastRoom.leftSocket < 0)
            {
                List<Room> validRooms2 = validRooms.Where(room => room.rightSocket == Math.Abs(lastRoom.leftSocket)).ToList();
                Room currentRoom = (Room)validRooms2[UnityEngine.Random.Range(0, validRooms2.Count)];
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x - ((currentRoom.width / 2) + (lastRoom.width / 2)), lastRoom.transform.position.y - currentRoom.inDoor, 0), Quaternion.identity);
            }
            else if(lastRoom.rightSocket < 0)
            {
                List<Room> validRooms2 = validRooms.Where(room => room.leftSocket == Math.Abs(lastRoom.rightSocket)).ToList();
                Room currentRoom = (Room)validRooms2[UnityEngine.Random.Range(0, validRooms2.Count)]; ;
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x + lastRoom.width + ((currentRoom.width / 2) - (lastRoom.width / 2)), lastRoom.transform.position.y - currentRoom.inDoor, 0), Quaternion.identity);
            }
            else if(lastRoom.topSocket < 0)
            {
                List<Room> validRooms2 = validRooms.Where(room => room.bottomSocket == Math.Abs(lastRoom.topSocket)).ToList();
                Room currentRoom = (Room)validRooms2[UnityEngine.Random.Range(0, validRooms2.Count)];
                newRoom = (Room)Instantiate(currentRoom, new Vector3(lastRoom.transform.position.x - currentRoom.inDoor, lastRoom.transform.position.y + lastRoom.height, 0), Quaternion.identity);
            }
            else if( lastRoom.bottomSocket < 0)
            {
                List<Room> validRooms2 = validRooms.Where(room => room.topSocket == Math.Abs(lastRoom.bottomSocket)).ToList();
                Room currentRoom = (Room)validRooms2[UnityEngine.Random.Range(0, validRooms2.Count)];
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

        List<Room> testRooms = generatedRooms.Where(room => room.transform.position.x > 0).ToList();
        //log all of the testRooms
        for (int i = 0; i < testRooms.Count; i++)
        {
            Debug.Log("Name: " + testRooms[i].transform.name + "Position: " + testRooms[i].transform.position.x + "width: " + testRooms[i].width);
        }


        Debug.Log("Finish room width: " + finishRoom.width);
        Debug.Log("Last room width: " + lastRoom.width);
        Debug.Log("Finish room height: " + finishRoom.height);
        Debug.Log("Last room height: " + lastRoom.height);


        if(lastRoom.rightSocket < 0)
        {
            Debug.Log("Placing finish room right");
            finalRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x + (lastRoom.width / 2) + (finishRoom.width / 2), lastRoom.transform.position.y + finishRoom.inDoor, 0), Quaternion.identity);
        }
        else if(lastRoom.leftSocket < 0)
        {
            Debug.Log("Placing finish room left");
            finalRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x - (lastRoom.width / 2) - (finishRoom.width / 2), lastRoom.transform.position.y + finishRoom.inDoor, 0), Quaternion.identity);
        }
        else if(lastRoom.topSocket < 0)
        {
            Debug.Log("Placing finish room top");
            finalRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x + finishRoom.inDoor, lastRoom.transform.position.y + lastRoom.height + 1, 0), Quaternion.identity);
        }
        else if(lastRoom.bottomSocket < 0)
        {
            Debug.Log("Placing finish room bottom");
            finalRoom = Instantiate(finishRoom, new Vector3(lastRoom.transform.position.x , lastRoom.transform.position.y - lastRoom.height + 1, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("No door found");
        }

        //modify the finish room to have the side which is connected to the last room be the door
        if(lastRoom.rightSocket < 0)
        {
            //modify the tilemap to have an opening on the left side, 4 tiles tall
            finalRoom.SetupRoom(0);
        }
        else if(lastRoom.leftSocket < 0)
        {
            finalRoom.SetupRoom(1);
        }
        else if(lastRoom.topSocket < 0)
        {
            finalRoom.SetupRoom(3);
        }
        else if(lastRoom.bottomSocket < 0)
        {
            finalRoom.SetupRoom(2);
        }
        else
        {
            Debug.Log("No door found");
        }
    }
}
