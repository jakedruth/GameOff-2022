using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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
