using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector3 direction = Vector3.zero;
    [SerializeField] Player player;

    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Camera _playerCamera;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
       
        float _inputX = Input.GetAxisRaw("Horizontal");
        float _inputY = Input.GetAxisRaw("Vertical");

        Vector3 _direction = new Vector3(_inputX, 0f, _inputY).normalized;

        _playerRigidbody.linearVelocity = _direction * player.speed;

        _playerCamera.transform.position = transform.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}