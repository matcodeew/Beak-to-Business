
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
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
    private float _playerSpeed;

    [Header("Mouse")]
    private Vector3 _mousePosition;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        EventManager.OnSkinChanged += SetComponent;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * _playerSpeed;

        if (_playerCamera)
        {
            _mousePosition = _playerCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 heading = _mousePosition - transform.position;
            float distance = heading.magnitude;
            direction = heading / distance;
        }
    }
    public Vector2 getPlayerInput() => _moveInput;
    public void GetPlayerSpeed(float Value)
    {
        _playerSpeed = Value;
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

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (context.started)
        {
            _animator.enabled = true;
            _animator.SetBool("IsMoving", true);
        }

        _animator.SetFloat("DirectionX", _moveInput.x);
        _animator.SetFloat("DirectionY", _moveInput.y);
        _skinAnimation.SaveLastFrame();

        if (context.canceled)
        {
            this.enabled = false;
            
            _moveInput = Vector2.zero;
            _rb.linearVelocity = Vector2.zero;
            _animator.SetBool("IsMoving", false);
            _skinAnimation.SetSprite();
        }
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
