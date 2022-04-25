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
            //StartCoroutine(SpawnCollectableCorrutine());
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnCollectable;
    }


    private IEnumerator SpawnCollectableCorrutine()
    {
        while (true)
        { 
            yield return new WaitForSeconds(3);
            SpawnCollectable();
        }
    }

    public void SpawnCollectable()
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
