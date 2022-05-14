using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerSpawner : NetworkBehaviour
{


    [SerializeField]
    public int numOfPlayers = 0;


    [SerializeField]
    private int limitOfPlayers = 2;

    [SerializeField]
    private List<Transform> basesTransform;
    [SerializeField]
    private List<GameObject> playerPrefabs;
    [SerializeField]
    private GameObject attchPoint;

    //public GameObject posUI;
    //public GameObject inGameUI;
    //public GameObject connectionUI;



    private void Setup()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = false;
        bool createPlayerObject = false;

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            callback(false, null, true, null, null);
            //connectionUI.SetActive(false);
            //posUI.SetActive(true);
            return;
        }

        if (limitOfPlayers > NetworkManager.Singleton.ConnectedClientsList.Count)
        {
            numOfPlayers++;
            Debug.Log("ApproveClient");
            approve = true;

            //connectionUI.SetActive(false);
            //posUI.SetActive(true);
            

        }

        callback(createPlayerObject, null, approve, Vector3.zero, Quaternion.identity);


    }

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }


    private void OnServerStarted()
    {
        if (NetworkManager.Singleton.IsServer)
        {

        }
    }

    public void SpawnPlayerByClick(int spawnIndex)
    {

        if (IsHost)
        {
            SpawnPlayer(NetworkManager.Singleton.LocalClientId, spawnIndex);
        }
        else if (IsClient)
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, spawnIndex);
        }

        //posUI.SetActive(false);
        //inGameUI.SetActive(true);
    }


    //[ServerRpc]
    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong clientId, int spawnPointId)
    {
        SpawnPlayer(clientId, spawnPointId);
    }

    private void SpawnPlayer(ulong clientId, int spawnPointId)
    {
        if (IsServer)
        {
            NetworkObject playerNO = Instantiate(playerPrefabs[spawnPointId], basesTransform[spawnPointId].position, Quaternion.identity).GetComponent<NetworkObject>();
            playerNO.SpawnAsPlayerObject(clientId);

            GameObject go = Instantiate(attchPoint);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.Spawn();
            no.TrySetParent(playerNO, false);

            //UpdateCamClientRpc(clientId, playerNO.transform);
        }
    }
    /*
    [ClientRpc]
    void UpdateCamClientRpc(ulong clientId, Transform lookAt)
    {
        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            Camera.main.GetComponent<CameraTransform>().lookat = transform;
        }
    }
    */
}
