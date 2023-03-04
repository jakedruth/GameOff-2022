using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public LevelNode startNode;
    public List<Path> paths;

    protected void OnDrawGizmosSelected()
    {
        Vector3 offset = Vector3.up * 0.17f;
        for (int i = 0; i < paths.Count; i++)
        {
            Gizmos.color = Color.blue;
            for (int j = 1; j < paths[i].points.Length; j++)
            {
                Vector3 p1 = paths[i].points[j - 1] + offset;
                Vector3 p2 = paths[i].points[j] + offset;
                Gizmos.DrawLine(p1, p2);
            }
        }
    }

    // protected void OnDrawGizmosSelected()
    // {
    //     Vector3 offset = Vector3.up * 0.17f;
    //     for (int i = 0; i < routes.Length; i++)
    //     {
    //         // Draw Path
    //         Gizmos.color = routes[i].routeUnlocked ? Color.green : Color.red;
    //         for (int j = 1; j < routes[i].path.Length; j++)
    //         {
    //             Vector3 p1 = routes[i].path[j - 1] + offset;
    //             Vector3 p2 = routes[i].path[j] + offset;
    //             Gizmos.DrawLine(p1, p2);
    //         }
    //     }
    // }

    // public List<Route> GetRoutesWithNode(LevelNode node)
    // {
    //     List<Route> returnRoutes = new();
    //     for (int i = 0; i < routes.Length; i++)
    //     {
    //         Route route = routes[i];
    //         if (route.nodeA == node || route.nodeB == node)
    //             returnRoutes.Add(route);
    //     }
    //     return returnRoutes;
    // }

    public void LoadLevel(LevelNode node)
    {
        Debug.Log($"[{node.name}] - Loading Level '{node.levelName}'");
    }
}

[System.Serializable]
public class Route
{
    public bool routeUnlocked;
    public LevelNode nodeA;
    public LevelNode nodeB;
    public CompassDirection directionA;
    public CompassDirection directionB;
    public Vector3[] path;
}

[System.Serializable]
public class Path
{
    public bool pathUnlocked;
    public Vector3[] points;

    public void GenerateStraightPath(LevelNode a, LevelNode b)
    {
        points = new[] { a.transform.position, b.transform.position };
    }
}

public enum CompassDirection
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
