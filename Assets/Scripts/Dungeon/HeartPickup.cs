using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : Pickup
{
    public override void HandleOnPlayerEnter(PlayerController pc)
    {
        pc.Actor.ApplyDamage(-1, true);
    }
}
