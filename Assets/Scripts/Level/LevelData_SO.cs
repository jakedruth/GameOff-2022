using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "GameOff-2022/Level Data", order = 0)]
public class LevelData_SO : ScriptableObject
{
    public RoomDetail_SO[] RoomDetails;
    public LevelBuildInstruction[] buildInstructions;
}

[System.Serializable]
public struct LevelBuildInstruction
{
    public RoomDoorPair roomA;
    public RoomDoorPair roomB;
    public int connectionType;

}

[System.Serializable]
public struct RoomDoorPair
{
    public int roomDetailIndex;
    public int doorID;
}
