using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JDR.ExtensionMethods;

public class LevelNode : MonoBehaviour
{
    public string levelName;
    public Trail northTrail;
    public Trail eastTrail;
    public Trail southTrail;
    public Trail westTrail;

    public Trail GetTrail(CompassDirection direction)
    {
        switch (direction)
        {
            case CompassDirection.NORTH:
                return northTrail;
            case CompassDirection.EAST:
                return eastTrail;
            case CompassDirection.SOUTH:
                return southTrail;
            case CompassDirection.WEST:
                return westTrail;
        }

        throw new System.Exception("I'm not even sure how you can get here...");
    }

    public bool SwapTrails(CompassDirection trailDirection1, CompassDirection trailDirection2)
    {
        if (trailDirection1 == trailDirection2)
            return false;

        Trail trail1 = GetTrail(trailDirection1);
        Trail trail2 = GetTrail(trailDirection2);
        Trail temp = new()
        {
            pathIndex = trail1.pathIndex,
            targetNode = trail1.targetNode,
            invertPath = trail1.invertPath
        };

        trail1.pathIndex = trail2.pathIndex;
        trail1.targetNode = trail2.targetNode;
        trail1.invertPath = trail2.invertPath;

        trail2.pathIndex = temp.pathIndex;
        trail2.targetNode = temp.targetNode;
        trail2.invertPath = temp.invertPath;

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 offset = Vector3.up * 0.17f;
        OverworldManager manager = FindObjectOfType<OverworldManager>();
        for (CompassDirection cd = CompassDirection.NORTH; cd <= CompassDirection.WEST; cd++)
        {
            Trail trail = GetTrail(cd);
            if (trail.pathIndex < 0 || trail.pathIndex >= manager.PathCount)
                continue;

#if UNITY_EDITOR
            GUI.color = Color.black;
            Vector3 textPos = transform.position + Vector3.up * 0.4f + cd.ToVector3() * 0.25f;
            UnityEditor.Handles.Label(textPos, cd.ToString()[0].ToString());
#endif

            Path path = manager.GetPath(trail);
            Gizmos.color = path.pathUnlocked ? Color.green : Color.red;
            for (int j = 1; j < path.Length; j++)
            {
                Vector3 p1 = path[j - 1] + offset;
                Vector3 p2 = path[j] + offset;
                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}

[System.Serializable]
public class Trail
{
    public int pathIndex = -1;
    public LevelNode targetNode;
    public bool invertPath;
}
