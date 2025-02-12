using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Sprite")]
    private Sprite _lastFrame;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SaveLastFrame()
    {
        _lastFrame = _spriteRenderer.sprite;
    }

    public void SetSprite()
    {
        if (_lastFrame is null)
        {
            _spriteRenderer.sprite = null;
            return;
        }
        _animator.enabled = false;
        _spriteRenderer.sprite = _lastFrame;
        _lastFrame = null;
    }
}
