using System;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
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
        transform.position += (Vector3)(_moveInput * _player.stats.speed * Time.fixedDeltaTime);
        animator.SetFloat("Speed", Mathf.Abs(_moveInput.x));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sr.sprite = _defaultSprite;
            animator.enabled = true;
            animator.SetBool("IsMoving", true);
        }

        _moveInput = context.ReadValue<Vector2>();
        SetBoolAnimation(_moveInput);
        animator.SetFloat("DirectionX", _moveInput.x);
        animator.SetFloat("DirectionY", _moveInput.y);

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

    public void SetBoolAnimation(Vector2 _moveInput)
    {
        if ((_moveInput.x > 0 && _moveInput.x < 1) && (_moveInput.y > 0 && _moveInput.y < 1)) // haut droite
        {
            print("haut droite");
            animator.SetBool("MoveUp", true);
            animator.SetBool("MoveRight", true);
            animator.SetBool("MoveDown", false);
            animator.SetBool("MoveLeft", false);
        }
        else if ((_moveInput.x < 0 && _moveInput.x > -1) && (_moveInput.y > 0 && _moveInput.y < 1)) // haut gauche
        {
            print("haut Gauche");
            animator.SetBool("MoveUp", true);
            animator.SetBool("MoveLeft", true);
            animator.SetBool("MoveDown", false);
            animator.SetBool("MoveRight", false);
        }
        else if ((_moveInput.x > 0 && _moveInput.x < 1) && (_moveInput.y < 0 && _moveInput.y > -1)) // bas droite
        {
            print("bas droite");
            animator.SetBool("MoveDown", true);
            animator.SetBool("MoveRight", true);
            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveLeft", false);
        }
        else if ((_moveInput.x < 0 && _moveInput.x > -1) && (_moveInput.y < 0 && _moveInput.y > -1)) // bas gauche
        {
            print("bas Gauche");
            animator.SetBool("MoveDown", true);
            animator.SetBool("MoveLeft", true);
            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveRight", false);
        }
        else if (_moveInput == Vector2.up)
        {
            animator.SetBool("MoveDown", false);
            animator.SetBool("MoveLeft", false);
            animator.SetBool("MoveUp", true);
            animator.SetBool("MoveRight", false);
        }
        else if ( _moveInput == Vector2.down)
        {
            animator.SetBool("MoveDown", true);
            animator.SetBool("MoveLeft", false);
            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveRight", false);
        }
        else if ( _moveInput == Vector2.right)
        {
            animator.SetBool("MoveDown", false);
            animator.SetBool("MoveLeft", false);
            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveRight", true);
        }
        else if ( _moveInput == Vector2.left)
        {
            animator.SetBool("MoveDown", false);
            animator.SetBool("MoveLeft", true);
            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveRight", false);
        }
    }
}