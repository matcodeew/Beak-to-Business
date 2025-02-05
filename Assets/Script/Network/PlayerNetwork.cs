using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerUI;

    public static event Action<GameObject> OnPlayerSpawn;
    public static event Action<GameObject> OnPlayerDespawn;

    private float _moveSpeed = 5f;

    private void Start()
    {
        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
            _playerUI.SetActive(true);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnPlayerSpawn?.Invoke(this.gameObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnPlayerDespawn?.Invoke(this.gameObject);
    }

}