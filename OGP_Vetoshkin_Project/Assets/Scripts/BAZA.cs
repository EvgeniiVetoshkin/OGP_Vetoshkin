using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BAZA : NetworkBehaviour
{
    public TextMeshPro textMeshPro;


    private void Start()
    {

        textMeshPro = GetComponentInChildren<TextMeshPro>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMove>().CubesDrop();


            }

            
           
        }

        if (other.CompareTag("Collectable"))
        {
            textMeshPro.text = (int.Parse(textMeshPro.text) + 1).ToString();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            textMeshPro.text = (int.Parse(textMeshPro.text) - 1).ToString();
        }
    }
}
