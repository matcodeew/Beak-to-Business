using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerUI;


    private float _moveSpeed = 5f;

    private void Start()
    {
        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
            _playerUI.SetActive(true);
        }
    }

}