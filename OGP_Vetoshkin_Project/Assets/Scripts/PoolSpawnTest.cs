using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PoolSpawnTest : NetworkBehaviour
{
    [SerializeField]
    private GameObject prefab;

    public void SpawnPooledObject()
    {
        if (IsServer)
        {
            NetworkObject no = NetworkObjectPool.Singleton.GetNetworkObject(prefab);
            no.Spawn();
            //NetworkObject.Spawn();
        }
    }
}
