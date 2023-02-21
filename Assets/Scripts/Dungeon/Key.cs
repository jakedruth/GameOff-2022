using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup
{
    public override void HandleOnPlayerEnter(PlayerController pc)
    {
        int current = pc.inventory.key.Get();
        pc.inventory.key.Set(current + 1);
        //Debug.Log($"Key count: {pc.KeyCount}");
    }
}
