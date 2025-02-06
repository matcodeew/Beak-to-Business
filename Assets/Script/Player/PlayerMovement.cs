using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct LastFrameDirection
{
   public Sprite Up;
   public Sprite Down;
   public Sprite Left;
   public Sprite Right;
}

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Player _player;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private LastFrameDirection _lastFrameDirection;
    private Vector2 _playerDirection;
    [SerializeField] private Sprite _lastFrame;
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
            print("Action started");
            animator.SetBool("IsMoving", true);
           // _playerDirection = context.ReadValue<Vector2>();
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);

        if (context.canceled)
        {
            print("Action canceled");
            this.enabled = false;
            animator.SetBool("IsMoving", false);
            sr.sprite = _lastFrame;
        }
    }


    private Sprite SetPlayerIdleSprite(Vector2 moveInput)
    {
        if (moveInput == Vector2.up)
        {
            return _lastFrameDirection.Up;
        }
        else if (moveInput == Vector2.down)
        {
            return _lastFrameDirection.Down;
        }
        else if (moveInput == Vector2.left)
        {
            return _lastFrameDirection.Left;
        }
        else if (moveInput == Vector2.right)
        {
            return _lastFrameDirection.Right;
        }
        return null;
    }

    public void SaveLastFrame()
    {
        _lastFrame = sr.sprite;
        print(_lastFrame);
    }

    public void SetSprite()
    {
        Debug.Log(_lastFrame);
        if(_lastFrame == null)
        {
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = _lastFrame;
        _lastFrame = null;
    }
}