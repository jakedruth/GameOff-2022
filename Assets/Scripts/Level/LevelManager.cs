using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;
    public LevelData_SO levelData;
    private Room[] _rooms;

    protected void Awake()
    {
        current = this;
        BuildLevel();
    }

    private void BuildLevel()
    {
        _rooms = new Room[levelData.RoomDetails.Length];
        for (int i = 0; i < levelData.buildInstructions.Length; i++)
        {
            // Get the instruction
            LevelBuildInstruction instruction = levelData.buildInstructions[i];

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
                // TODO: Handle if a room needs to be rotated

                Vector3 localDelta = doorA.localPosition - doorB.localPosition + doorA.forward;
                roomB.transform.position = roomA.transform.position + localDelta;
            }

            // Handle the connection type
            doorA.gameObject.SetActive(false);
            doorB.gameObject.SetActive(false);
        }
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
        return Instantiate(levelData.RoomDetails[roomDetailIndex].room);
    }

    public void UnlockOverworldPath(int ID)
    {
        Debug.Log($"<color=green>Unlocking</color> path [{ID}]");
        GameManager.Instance.gameData.unlockedPaths[ID] = true;
    }

    public void LockOverworldPath(int ID)
    {

        Debug.Log($"<color=red>Locking</color> path [{ID}]");
        GameManager.Instance.gameData.unlockedPaths[ID] = false;
    }

    public void ExitLevel()
    {
        Debug.Log("Exiting Level");
        GameManager.Instance.LoadScene(1);
    }
}
