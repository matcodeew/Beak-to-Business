using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
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
        if (_spriteRenderer != null)
        {
            _lastFrame = _spriteRenderer.sprite;
        }
    }

    public void SetSprite()
    {
        _animator.enabled = false;
        _spriteRenderer.sprite = _lastFrame;
    }

    //[ServerRpc]
    //public void SaveLastFrameServerRpc(Vector2 playerMovement)
    //{
    //    if (_spriteRenderer != null)
    //    {
    //        _lastFrame = _spriteRenderer.sprite;
    //        if (playerMovement == Vector2.zero)
    //        {
    //            SetSpriteClientRpc(_lastFrame);
    //        }
    //    }
    //}

    //[ClientRpc]
    //private void SetSpriteClientRpc(Sprite newSprite)
    //{
    //    if (_spriteRenderer == null)
    //        return;

    //    _animator.enabled = false;
    //    _spriteRenderer.sprite = newSprite;
    //}
}
