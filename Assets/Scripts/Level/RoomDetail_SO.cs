using UnityEngine;

[CreateAssetMenu(fileName = "RoomDetail", menuName = "GameOff-2022/Room Detail", order = 1)]
public class RoomDetail_SO : ScriptableObject
{
    public Room room;
    public RoomModifier[] modifiers;
}

[System.Serializable]
public class RoomModifier
{
    public int ModifierType;
    public string testString;
    public bool testBool;
}