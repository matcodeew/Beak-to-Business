using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _direction = Vector2.zero;
    private Player _player;

    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        //if(gamePaused || playerDead) return;

        float _inputX = Input.GetAxisRaw("Horizontal");
        float _inputY = Input.GetAxisRaw("Vertical");
       // print($"intputX {_inputX}, intputY {_inputY}");
        _direction = new Vector3(_inputX, 0.0f, _inputY).normalized;

        _playerRigidbody.linearVelocity = _direction * _player.stats.speed;
    }
}