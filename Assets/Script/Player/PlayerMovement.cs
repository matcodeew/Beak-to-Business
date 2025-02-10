using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Player _player;
    [SerializeField] Animator animator;
    private Vector2 _playerDirection;
    [HideInInspector] public Vector3 direction = Vector2.zero;
    [SerializeField] private Camera _playerCamera;
    private Rigidbody2D _rb;
    private Vector3 _mousePosition;

    private GameObject _currentSkin; 
    private PlayerAnimation _skinAnimation; 
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _rb = GetComponentInParent<Rigidbody2D>();
        Player.OnSkinChanged += SetComponent;
    }

    private void SetComponent()
    {
        _currentSkin = GetPlayerSkin();
        _skinAnimation = _currentSkin.GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(_moveInput * (_player.stats.speed * Time.fixedDeltaTime));
        //direction = _mousePosition - transform.position;
        if (_playerCamera)
        {
            _mousePosition = _playerCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 heading = _mousePosition - transform.position;
            float distance = heading.magnitude;
            direction = heading / distance;
        }
    }

    public void SetRightAnimator(GameObject choosenSkin)
    {
        animator = choosenSkin.GetComponent<Animator>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.enabled = true;
            animator.SetBool("IsMoving", true);
        }

        _moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("DirectionX", _moveInput.x);
        animator.SetFloat("DirectionY", _moveInput.y);

        _skinAnimation.SaveLastFrame();
        if (context.canceled)
        {
            this.enabled = false;
            animator.SetBool("IsMoving", false);
            _skinAnimation.SetSprite();
        }
    }

    public GameObject GetPlayerSkin() //Warning: Skin must be on the top of the player hierarchy
    {
        Transform skinParent = transform.GetChild(0); //change the position in the hierarchy here
        for(int i = 0; i < skinParent.childCount; i++)
        {
            if (skinParent.GetChild(i).gameObject.activeSelf)
            {
                return skinParent.GetChild(i).gameObject;
            }
        }
        return null;
    }
}