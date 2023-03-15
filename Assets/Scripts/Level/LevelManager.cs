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
        levelData.BuildLevel(this);
    }

    public void SetRoomsCollection(Room[] rooms)
    {
        _rooms = rooms;
        foreach (Room r in _rooms)
        {
            if (r == null)
                continue;
            r.transform.SetParent(transform);
        }
    }

    public Room GetRoom(int index)
    {
        return _rooms[index];
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
