using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : Pickup
{
    public override void HandleOnPlayerEnter(PlayerController pc)
    {
        pc.actor.ApplyDamage(-1, true);
    }
}
