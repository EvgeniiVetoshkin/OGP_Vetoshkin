using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.UI;

public class IPConnect : MonoBehaviour
{
    public UNetTransport uNetTransport;
    public InputField iPinmut;
    public InputField port;


    private void Start()
    {
        uNetTransport = GetComponent<UNetTransport>();

    }

    public void ConnectToIP()
    {
        uNetTransport.ConnectAddress = iPinmut.text;
        uNetTransport.ConnectPort = int.Parse(port.text);

        NetworkManager.Singleton.StartClient();
    }

}
