using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Player _player;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void FixedUpdate()
    {
        transform.position += (Vector3)(moveInput * _player.stats.speed * Time.fixedDeltaTime);
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetBool("IsMoving", true);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);
        if (context.canceled)
        {
            sr.flipX = moveInput.x < 0 ? true : false;
            //sr.flipY = moveInput.y < 0 ? true : false;
            animator.SetBool("IsMoving", false);
            this.enabled = false;
        }
    }
}
