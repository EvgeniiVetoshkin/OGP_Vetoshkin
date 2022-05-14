using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerButtons : MonoBehaviour
{
    //public GameObject connectionUI;
    //public GameObject posUI;


    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
