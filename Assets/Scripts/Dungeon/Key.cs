using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup
{
    public override void OnPlayerEnter(PlayerController pc)
    {
        pc.keys++;
        Debug.Log($"Key count: {pc.keys}");
    }
}
