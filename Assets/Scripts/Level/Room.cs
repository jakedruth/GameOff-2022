using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomTrigger roomTrigger { get; private set; }
    public Transform[] doors;

    protected void Reset()
    {
        roomTrigger = GetComponentInChildren<RoomTrigger>();

        if (roomTrigger == null)
        {
            GameObject temp = new("RoomTrigger");
            roomTrigger = temp.AddComponent<RoomTrigger>();
            roomTrigger.name = "RoomTrigger";
            roomTrigger.transform.SetParent(transform);
        }
    }
}
