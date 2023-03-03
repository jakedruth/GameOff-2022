using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public Route[] routes;

    protected void OnDrawGizmosSelected()
    {
        Vector3 offset = Vector3.up * 0.17f;
        for (int i = 0; i < routes.Length; i++)
        {
            // Draw Path
            Gizmos.color = routes[i].routeUnlocked ? Color.green : Color.red;
            for (int j = 1; j < routes[i].path.Length; j++)
            {
                Vector3 p1 = routes[i].path[j - 1] + offset;
                Vector3 p2 = routes[i].path[j] + offset;
                Gizmos.DrawLine(p1, p2);
            }
        }
    }

    public List<Route> GetRoutesWithNode(LevelNode node)
    {
        List<Route> returnRoutes = new();
        for (int i = 0; i < routes.Length; i++)
        {
            Route route = routes[i];
            if (route.nodeA == node || route.nodeB == node)
                returnRoutes.Add(route);
        }
        return returnRoutes;
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

public enum CompassDirection
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
