using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput;
    private Player _player;

    Rigidbody2D _rb;

    private Vector3 _mousePosition = Vector2.zero;

    [HideInInspector] public Vector3 direction = Vector2.zero;
    [SerializeField] private Camera _playerCamera;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    { 
        transform.position += (Vector3)(moveInput * _player.stats.speed * Time.fixedDeltaTime);
        //direction = _mousePosition - transform.position;
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
        moveInput = context.ReadValue<Vector2>();
        //if (moveInput != Vector2.zero) direction = moveInput;

        //if (context.canceled)
        //{
        //    this.enabled = false;
        //}
    }

    public void OnMouseMouve(InputAction.CallbackContext ctx)
    {
        //_mousePosition = ctx.ReadValue<Vector2>();
    }
}
