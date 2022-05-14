using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class PlayerNameSet : NetworkBehaviour
{
    [SerializeField]
    private Text nameText;
    //private TMPro nameText;
    Button buttonComponent;
    InputField inputFieldComponent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            GameObject button = GameObject.Find("SubmitNameButton");
            GameObject input = GameObject.Find("TextInput");
            buttonComponent = button.GetComponent<Button>();
            inputFieldComponent = input.GetComponent<InputField>();
            buttonComponent.onClick.AddListener(SendNametoServer);

        }

    }

    private void SendNametoServer()
    {
        if (IsOwner)
        {
            SetNameServerRPC(inputFieldComponent.text);
        }
    }
    [ServerRpc]
    private void SetNameServerRPC(string message)
    {
        SetNewClientRPC(message);
    }
    [ClientRpc]
    private void SetNewClientRPC(string message)
    {
        nameText.text = message;;
    }
}
