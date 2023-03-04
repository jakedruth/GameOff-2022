using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelNode : MonoBehaviour
{
    public string levelName;
    public Optional<Trail> northTrail;
    public Optional<Trail> eastTrail;
    public Optional<Trail> southTrail;
    public Optional<Trail> westTrail;

    public Trail GetTrail(CompassDirection direction)
    {
        switch (direction)
        {
            case CompassDirection.NORTH:
                return northTrail.Enabled ? northTrail.Value : null;
            case CompassDirection.EAST:
                return eastTrail.Enabled ? eastTrail.Value : null;
            case CompassDirection.SOUTH:
                return southTrail.Enabled ? southTrail.Value : null;
            case CompassDirection.WEST:
                return westTrail.Enabled ? westTrail.Value : null;
        }

        throw new System.Exception("I'm not even sure how you can get here...");
    }
}

[System.Serializable]
public class Trail
{
    public LevelNode node;
    public int pathIndex;
    public bool invertPath;
}
