using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public LevelNode startNode;
    [SerializeField] private List<Path> paths;
    public int PathCount { get { return paths.Count; } }

    protected void OnDrawGizmosSelected()
    {
        Vector3 offset = Vector3.up * 0.1f;
        for (int i = 0; i < paths.Count; i++)
        {
            Gizmos.color = Color.blue;
            for (int j = 1; j < paths[i].points.Length; j++)
            {
                Vector3 p1 = paths[i].points[j - 1] + offset;
                Vector3 p2 = paths[i].points[j] + offset;
                Gizmos.DrawLine(p1, p2);
#if UNITY_EDITOR
                UnityEditor.Handles.Label((p1 + p2) * 0.5f, i.ToString());
#endif
            }
        }
    }

    public Path GetPath(Trail trail)
    {
        if (trail == null)
            return null;

        return GetPath(trail.pathIndex);
    }

    public Path GetPath(int index)
    {
        if (index < 0 || index >= paths.Count)
            return null;

        return paths[index];
    }

    public void LoadLevel(LevelNode node)
    {
        Debug.Log($"[{node.name}] - Loading Level '{node.levelName}'");
    }
}

// [System.Serializable]
// public class Route
// {
//     public bool routeUnlocked;
//     public LevelNode nodeA;
//     public LevelNode nodeB;
//     public CompassDirection directionA;
//     public CompassDirection directionB;
//     public Vector3[] path;
// }

[System.Serializable]
public class Path
{
    public bool pathUnlocked;
    public Vector3[] points;

    public void GenerateStraightPath(LevelNode a, LevelNode b)
    {
        points = new[] { a.transform.position, b.transform.position };
    }

    public Vector3 this[int index]
    {
        get
        {
            return points[index];
        }

        set
        {
            points[index] = value;
        }
    }

    public int Length { get { return points.Length; } }
}

public enum CompassDirection
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
