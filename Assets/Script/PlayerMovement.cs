using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _direction = Vector2.zero;
    [SerializeField] private Player _player;

    [SerializeField] private Rigidbody _playerRigidbody;
    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        //if(gamePaused || playerDead) return;

        float _inputX = Input.GetAxisRaw("Horizontal");
        float _inputY = Input.GetAxisRaw("Vertical");
       // print($"intputX {_inputX}, intputY {_inputY}");
        _direction = new Vector3(_inputX, 0.0f, _inputY).normalized;

        _playerRigidbody.linearVelocity = _direction * _player.stats.speed;

        _playerCamera.transform.position = transform.position + new Vector3(0, 10, 0);
        _playerCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}