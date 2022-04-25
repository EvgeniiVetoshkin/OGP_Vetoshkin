using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public enum CollectableState
{
    Untouched,
    Picked,
    Delivered
}

public class Collectable : NetworkBehaviour
{
    private NetworkVariable<CollectableState> collectableState = new NetworkVariable<CollectableState>(CollectableState.Untouched);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (IsServer && transform.parent != other.transform && other.tag == "Player" )
        {
            transform.parent = other.transform;
        }


    }*/

    
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != collision.transform && collectableState.Value == CollectableState.Untouched && collision.collider.tag == "Player")
        {
            collectableState.Value = CollectableState.Picked;
            transform.parent = collision.transform;
        }
        /*
        if (collectableState.Value == CollectableState.Picked && collision.collider.tag == "Base")
        {
            collectableState.Value = CollectableState.Delivered;
            transform.parent = null;
            transform.position = collision.transform.position + Vector3.up;
        }
        */
    }
    
}
