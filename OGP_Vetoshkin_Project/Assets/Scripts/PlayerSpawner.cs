using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    public int numOfPlayers;
    
    /*
    private void Setup()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {

        //Your logic here
        bool approve = true;
        bool createPlayerObject = true;

        // Position to spawn the player object at, set to null to use the default position
        Vector3? positionToSpawnAt = Vector3.zero;

        // Rotation to spawn the player object at, set to null to use the default rotation
        Quaternion rotationToSpawnWith = Quaternion.identity;

        //If approve is true, the connection gets added. If it's false. The client gets disconnected
        callback(createPlayerObject, null, approve, positionToSpawnAt, rotationToSpawnWith);
    }
    */

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GameObject go = Instantiate(playerPrefab);
                NetworkObject no = go.GetComponent<NetworkObject>();
                no.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject go = Instantiate(playerPrefab);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.SpawnAsPlayerObject(clientID);
        } 
    }
}
