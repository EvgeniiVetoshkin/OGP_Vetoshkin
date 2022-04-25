using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct PlayerStats
{
    //List<GameObject> collectables;
    //public GameObject[] collectables;
    public float speed;
}
public enum PlayerState
{ 
    Idle,
    Carry
}

public class PlayerMove : NetworkBehaviour
{
    [SerializeField]
    //private float movementSpeed = 5;
    private NetworkVariable<float> movementSpeed = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 5f);
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private NetworkVariable<PlayerState> playerState = new NetworkVariable<PlayerState>(PlayerState.Idle);
    [SerializeField]
    private NetworkVariable<PlayerStats> playerStats = new NetworkVariable<PlayerStats>();


    //PlayerStats playerStats = new PlayerStats();
    
    void Update()
    {
        if (IsOwner)
        {
            Vector3 movementDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movementDirection.z++;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementDirection.x--;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementDirection.z--;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementDirection.x++;
            }

            movementDirection = movementDirection.normalized;
            transform.LookAt(transform.position + movementDirection);


            //characterController.Move(movementDirection * Time.deltaTime * movementSpeed.Value);
            transform.localPosition += movementDirection * Time.deltaTime * movementSpeed.Value;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer && collision.collider.CompareTag("Collectable"))
        {
            //playerState.Value != PlayerState.Carry &&
            movementSpeed.Value *= 0.6f;
            playerState.Value = PlayerState.Carry;
        }
    }
}
