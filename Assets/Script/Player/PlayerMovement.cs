using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Player _player;
    [SerializeField] Animator animator;
    //[SerializeField] SpriteRenderer sr;
    private Vector2 _playerDirection;

    [Header("Sprite")]
    //[SerializeField] private Sprite _lastFrame;
    //[SerializeField] private Sprite _defaultSprite;

    [HideInInspector] public Vector3 direction = Vector2.zero;
    [SerializeField] private Camera _playerCamera;
    private Rigidbody2D _rb;
    private Vector3 _mousePosition;
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _rb = GetComponentInParent<Rigidbody2D>();
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
            //sr.sprite = _defaultSprite;
            animator.enabled = true;
            animator.SetBool("IsMoving", true);
        }

        _moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("DirectionX", _moveInput.x);
        animator.SetFloat("DirectionY", _moveInput.y);

        if (context.canceled)
        {
            this.enabled = false;
            animator.SetBool("IsMoving", false);
        }
    }

    
}