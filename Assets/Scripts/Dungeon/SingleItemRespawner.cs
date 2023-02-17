using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleItemRespawner : MonoBehaviour
{
    [SerializeField] private Pickup pickupPrefab;
    private Pickup _instance;
    private bool _isPickedUp;

    public void TrySpawnItem()
    {
        // check if the item has been picked up yet
        if (_isPickedUp)
            return;

        // check if there is an instance already
        if (_instance == null)
        {
            // create an instance and add a listener
            _instance = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            _instance.OnPickedUp.AddListener(ItemPickedUp);
        }
        else
        {
            // move the existing instance and don't add a listener
            _instance.transform.position = transform.position;
            _instance.Start();
        }
    }

    public void ItemPickedUp()
    {
        _isPickedUp = true;
    }
}
