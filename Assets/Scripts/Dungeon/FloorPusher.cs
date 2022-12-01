using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPusher : MonoBehaviour
{
    protected static List<FloorPusher> pushersAffectingPlayer = new();
    protected static Dictionary<Rigidbody, List<FloorPusher>> pushersAffectingBodies = new();

    [SerializeField] private float _pushSpeed;

    void Update()
    {
        if (pushersAffectingPlayer.Count > 0)
        {
            Vector3 dir = Vector3.zero;
            float maxPushSpeed = 0;
            foreach (FloorPusher pusher in pushersAffectingPlayer)
            {
                dir += pusher.transform.forward * pusher._pushSpeed;
                maxPushSpeed = Mathf.Max(pusher._pushSpeed, maxPushSpeed);
            }
            dir.Normalize();

            // Set the player's push speed
            PlayerController.instance.SetFloorPusherVelocity(dir * maxPushSpeed);

            pushersAffectingPlayer.Clear();
        }

        if (pushersAffectingBodies.Count > 0)
        {
            foreach (KeyValuePair<Rigidbody, List<FloorPusher>> pair in pushersAffectingBodies)
            {
                // extract the key-value pair
                Rigidbody rb = pair.Key;
                List<FloorPusher> pushers = pair.Value;

                if (rb == null)
                    continue;

                // calculate the move direction
                Vector3 dir = Vector3.zero;
                float maxPushSpeed = 0;
                foreach (FloorPusher pusher in pushers)
                {
                    dir += pusher.transform.forward * pusher._pushSpeed;
                    maxPushSpeed = Mathf.Max(pusher._pushSpeed, maxPushSpeed);
                }
                dir.Normalize();

                // move the rigidbody
                Vector3 pos = rb.transform.position;
                pos += dir * maxPushSpeed * Time.deltaTime;
                rb.MovePosition(pos);
            }

            pushersAffectingBodies.Clear();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePushPlayer(other.GetComponent<PlayerController>());
            return;
        }

        HandlePushRigidBody(other.GetComponent<Rigidbody>());
    }

    private void HandlePushPlayer(PlayerController playerController)
    {
        if (playerController == null)
            return;

        pushersAffectingPlayer.Add(this);
    }

    private void HandlePushRigidBody(Rigidbody rigidbody)
    {
        if (rigidbody == null)
            return;

        if (pushersAffectingBodies.ContainsKey(rigidbody))
            pushersAffectingBodies[rigidbody].Add(this);
        else
            pushersAffectingBodies.Add(rigidbody, new List<FloorPusher>() { this });


        // rigidbody.AddForce(transform.forward * _pushSpeed);
        // Vector3 pos = rigidbody.transform.position;
        // pos += transform.forward * _pushSpeed * Time.deltaTime;
        // rigidbody.MovePosition(pos);
    }
}
