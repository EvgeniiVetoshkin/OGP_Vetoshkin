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


    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collectableState.Value == CollectableState.Untouched && collision.collider.tag == "Player" && transform.parent != collision.transform)
            {
                collectableState.Value = CollectableState.Picked;
                GetComponent<Rigidbody>().isKinematic = true;

                PlayerMove playerMove = collision.collider.GetComponent<PlayerMove>();
                playerMove.OnCollectablePick();

                GetComponent<NetworkObject>().TrySetParent(collision.transform.GetComponentInChildren<AttachPointChange>().transform);

                //transform.parent = collision.transform.GetComponentInChildren<AttachPointChange>().transform;
                transform.localPosition = Vector3.zero + Vector3.up * 0.55f * (playerMove.cubesAmount.Value - 1) + Vector3.up * 0.2f + Vector3.forward * 0.1f;



                if (playerMove.cubesAmount.Value > 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(-90f, 90f), 0));
                }
                else
                {
                    transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

}
