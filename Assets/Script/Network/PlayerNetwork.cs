using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private PlayerInput _playerInputs;
    [SerializeField] private GameObject _healtBar;


    public static event Action<GameObject, ulong> OnPlayerSpawn;
    public static event Action<GameObject> OnPlayerDespawn;
    private void Start()
    {
        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
            _playerUI.SetActive(true);
            _playerInputs.enabled = true;
            _healtBar.SetActive(true);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnPlayerSpawn?.Invoke(this.gameObject, OwnerClientId);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnPlayerDespawn?.Invoke(this.gameObject);
    }

}