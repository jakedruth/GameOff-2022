using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup
{
    public override void handleOnPlayerEnter(PlayerController pc)
    {
        pc.KeyCount++;
        //Debug.Log($"Key count: {pc.KeyCount}");
    }
}
