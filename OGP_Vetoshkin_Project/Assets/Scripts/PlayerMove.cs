using Unity.Netcode;
using UnityEngine;

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
    private NetworkVariable<float> limitMovementSpeed = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 5f);

    [SerializeField]
    private NetworkVariable<float> maxMovementSpeed = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 5f);

    public NetworkVariable<int> cubesAmount = new NetworkVariable<int>(NetworkVariableReadPermission.Everyone, 0);

    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float acceleration = 4f;

    [SerializeField]
    [Tooltip("Speed Reduction for 1 cube in percentages")]
    private float speedReduction = 0.6f;


    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private NetworkVariable<PlayerState> playerState = new NetworkVariable<PlayerState>(PlayerState.Idle);
    [SerializeField]
    private NetworkVariable<PlayerStats> playerStats = new NetworkVariable<PlayerStats>();

    Animator animator;
    private int isCarryingHash;

    //[SerializeField]
    //public Transform attachPoint;


    //PlayerStats playerStats = new PlayerStats();

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

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
            if (Input.GetKey(KeyCode.Space))
            {
                CubesDrop();
            }

            if (movementDirection == Vector3.zero)
            {
                if (velocity.magnitude > 0.01f)
                {
                    movementDirection = -velocity;
                }
            }
            else
            {
                transform.LookAt(transform.position + velocity);
            }

            movementDirection = movementDirection.normalized;

            velocity = velocity + movementDirection * Time.deltaTime * acceleration;
            velocity = Vector3.ClampMagnitude(velocity, maxMovementSpeed.Value);


            transform.localPosition += velocity * Time.deltaTime;

            UpdateAnimServerRpc(velocity.magnitude, "Velocity");
            Camera.main.GetComponent<CameraTransform>().lookat = transform;
        }

    }

    public void CubesDrop()
    {
        if (IsServer)
        {
            cubesAmount.Value = 0;
            playerState.Value = PlayerState.Idle;
            UpdateAnimServerRpc(false, "IsCarrying");

            maxMovementSpeed.Value = (limitMovementSpeed.Value / 2) + (limitMovementSpeed.Value / 2) * Mathf.Pow(speedReduction, cubesAmount.Value);

            Transform attachPoint = GetComponentInChildren<AttachPointChange>().transform;
            foreach (Transform cubic in attachPoint)
            {
                cubic.GetComponent<Rigidbody>().isKinematic = false;
                cubic.parent = null;
            }
            

        }
    }

    public void OnCollectablePick()
    {
        if (IsServer)
        {
            cubesAmount.Value++;
            playerState.Value = PlayerState.Carry;
            UpdateAnimServerRpc(true, "IsCarrying");
            maxMovementSpeed.Value = (limitMovementSpeed.Value / 2) + (limitMovementSpeed.Value / 2) * Mathf.Pow(speedReduction, cubesAmount.Value);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void UpdateAnimServerRpc(bool change, string name)
    {
        UpdateAnimClientRpc(change, name);
    }
    [ServerRpc(RequireOwnership = false)]
    void UpdateAnimServerRpc(float change, string name)
    {
        UpdateAnimClientRpc(change, name);
    }

    [ClientRpc]
    void UpdateAnimClientRpc(bool change, string name)
    {
        animator.SetBool(name, change);
    }
    [ClientRpc]
    void UpdateAnimClientRpc(float change, string name)
    {
        animator.SetFloat(name, change);
    }
    /*
    private void OnEnable()
    {
        if (NetworkManager.Singleton.LocalClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId)
        { 
            Camera.main.GetComponent<CameraTransform>().lookat = transform;
        }
        if (IsOwner)
        {
        }
    }

    private void OnDisable()
    {
        if (IsOwner)
        {
            //Camera.main.GetComponent<CameraTransform>().lookat = null;
        }
    }
    */
}
