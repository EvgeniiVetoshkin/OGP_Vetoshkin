using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ColllectableSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject spawnCollectablePrefab;




    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnCollectable();
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnCollectable;
    }

    

    private void SpawnCollectable()
    {

        if (IsServer)
        {
            GameObject go = Instantiate(spawnCollectablePrefab);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.Spawn();
            go.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
