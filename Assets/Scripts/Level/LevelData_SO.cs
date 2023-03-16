using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "GameOff-2022/Level Data", order = 0)]
public class LevelData_SO : ScriptableObject
{
    public RoomDetail_SO[] RoomDetails;
    public LevelBuildInstruction[] buildInstructions;
    [HideInInspector] private Room[] _rooms;

    public void BuildLevel(LevelManager owner)
    {
        _rooms = new Room[RoomDetails.Length];
        for (int i = 0; i < buildInstructions.Length; i++)
        {
            // Get the instruction
            LevelBuildInstruction instruction = buildInstructions[i];

            // Save if the room has been built
            bool aIsBuilt = IsRoomBuilt(instruction.roomA.roomDetailIndex);
            bool bIsBuilt = IsRoomBuilt(instruction.roomB.roomDetailIndex);

            // Get references
            Room roomA = GetRoom(instruction.roomA.roomDetailIndex);
            Room roomB = GetRoom(instruction.roomB.roomDetailIndex);
            Transform doorA = roomA.doors[instruction.roomA.doorID];
            Transform doorB = roomB.doors[instruction.roomB.doorID];

            // Warning if roomA does not exist unless it is the first instruction
            if (i == 0)
                roomA.transform.position = Vector3.zero;
            else if (!aIsBuilt)
                Debug.LogWarning($"Not Sure where to place room [{instruction.roomA.roomDetailIndex}]:{roomA}");

            // Check to see if roomB needs to be moved
            if (!bIsBuilt)
            {
                // Rotate roomB so the doors are facing each other
                float alpha = (Vector3.SignedAngle(doorA.forward, doorB.forward, Vector3.up) - 180) % 360;
                roomB.transform.rotation = Quaternion.AngleAxis(alpha, Vector3.down); // Not entirely sure why this needs to be Vector3.down

                // Calculate the difference between the door and it's room in world space
                Vector3 diffA = doorA.transform.position - roomA.transform.position;
                Vector3 diffB = doorB.transform.position - roomB.transform.position;
                Vector3 displacement = diffA - diffB + doorA.forward;
                roomB.transform.position = roomA.transform.position + displacement;
            }

            // Handle the connection type
            doorA.gameObject.SetActive(false);
            doorB.gameObject.SetActive(false);
        }

        owner.SetRoomsCollection(_rooms);
    }

    private bool IsRoomBuilt(int roomIndex)
    {
        return _rooms[roomIndex] != null;
    }

    private Room GetRoom(int roomDetailIndex)
    {
        if (_rooms[roomDetailIndex] != null)
            return _rooms[roomDetailIndex];

        return _rooms[roomDetailIndex] = BuildRoom(roomDetailIndex);
    }

    private Room BuildRoom(int roomDetailIndex)
    {
        return Instantiate(RoomDetails[roomDetailIndex].room);
    }
}

[System.Serializable]
public struct LevelBuildInstruction
{
    public RoomDoorPair roomA;
    public RoomDoorPair roomB;
    public RoomConnection connectionType;
}

public enum RoomConnection
{
    EMPTY,
    DOOR,
    LOCKED,
    ONE_WAY,
    BREAKABLE
}

[System.Serializable]
public struct RoomDoorPair
{
    public int roomDetailIndex;
    public int doorID;
}
