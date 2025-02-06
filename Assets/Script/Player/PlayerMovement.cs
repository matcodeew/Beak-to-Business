using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput;
    private Player _player;

    Rigidbody2D _rb;

    [HideInInspector] public Vector2 direction = Vector2.zero;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //_rb.linearVelocity = moveInput * _player.stats.speed;
        transform.position += (Vector3)(moveInput * _player.stats.speed * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero) direction = moveInput;

        if (context.canceled)
        {
            this.enabled = false;
        }
    }
}
