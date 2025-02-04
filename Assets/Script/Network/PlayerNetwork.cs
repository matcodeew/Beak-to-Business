using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;


    private float _moveSpeed = 5f;

    private void Start()
    {
        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Call test Server Rpc ?  ");
            TestServerRpc();
        }
    }


    [ServerRpc]
    public void TestServerRpc()
    {
        Debug.Log("TestServerRpc called!");
    }

}