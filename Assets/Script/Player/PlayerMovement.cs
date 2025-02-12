using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    private PlayerAnimation _skinAnimation;

    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;

    [Header("Movement")]
    [HideInInspector] public Vector3 direction;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;

    [Header("Player")]
    private GameObject _currentSkin;
    private float _playerSpeed = 5f;

    [Header("Mouse")]
    private Vector3 _mousePosition;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        EventManager.OnSkinChanged += SetComponent;
    }


    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner)
    //    {
    //        enabled = false;
    //        return;
    //    }
    //}

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _rb.linearVelocity = _moveInput * _playerSpeed;

        if (_playerCamera)
        {
            _mousePosition = _playerCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 heading = _mousePosition - transform.position;
            float distance = heading.magnitude;
            direction = heading / distance;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        _moveInput = context.ReadValue<Vector2>();

        if (context.started)
        {
            _animator.enabled = true;
            _animator.SetBool("IsMoving", true);
        }

        if (context.canceled)
        {
            _moveInput = Vector2.zero;
            _rb.linearVelocity = Vector2.zero;
            _animator.SetBool("IsMoving", false);
        }

        UpdateAnimationServerRpc(_moveInput);
    }

    [ServerRpc]
    private void UpdateAnimationServerRpc(Vector2 moveInput)
    {
        UpdateAnimationClientRpc(moveInput);
    }

    public void SetPlayerSpeed(float Value)
    {
        _playerSpeed = Value;
    }

    [ClientRpc]
    private void UpdateAnimationClientRpc(Vector2 moveInput)
    {
        if (_animator == null) return;

        _animator.SetFloat("DirectionX", moveInput.x);
        _animator.SetFloat("DirectionY", moveInput.y);

        bool isMoving = moveInput != Vector2.zero;
        _animator.SetBool("IsMoving", isMoving);
    }

    private void SetComponent()
    {
        _currentSkin = GetPlayerSkin();
        _skinAnimation = _currentSkin.GetComponent<PlayerAnimation>();
    }

    public void SetRightAnimator(GameObject choosenSkin)
    {
        _animator = choosenSkin.GetComponent<Animator>();
    }

    public GameObject GetPlayerSkin()
    {
        Transform skinParent = transform.GetChild(0);
        for (int i = 0; i < skinParent.childCount; i++)
        {
            if (skinParent.GetChild(i).gameObject.activeSelf)
            {
                return skinParent.GetChild(i).gameObject;
            }
        }
        return null;
    }
}