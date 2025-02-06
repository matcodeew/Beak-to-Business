using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Player _player;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;
    private Vector2 _playerDirection;

    [Header("Sprite")]
    [SerializeField] private Sprite _lastFrame;
    [SerializeField] private Sprite _defaultSprite;
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
            sr.sprite = _defaultSprite;
            animator.enabled = true;
            animator.SetBool("IsMoving", true);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);

        if (context.canceled)
        {
            this.enabled = false;
            animator.SetBool("IsMoving", false);
        }
    }
    public void SaveLastFrame()
    {
        _lastFrame = sr.sprite;

        print($"save {_lastFrame} for {sr.sprite}");
    }

    public void SetSprite()
    {
        print($"set {_lastFrame} to {sr.sprite}");
        if (_lastFrame == null)
        {
            sr.sprite = null;
            return;
        }
        animator.enabled = false;
        sr.sprite = _lastFrame;
        _lastFrame = null;
    }
}