using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    public void Shoot(InputAction.CallbackContext _callback)
    {
        if (_player.weaponEquipied is null) { return; }

        if (_callback.started)
        {
            _player.weaponEquipied.Shoot(transform);
            print($"{_player.weaponEquipied.name} Shoot");
        }

        if (_callback.canceled)
        {
            _player.weaponEquipied.ShootFinished();
            print($"{_player.weaponEquipied.name} ShootFinished");
        }
    }
}
