using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    public void Shoot(InputAction.CallbackContext _callback)
    {
        if (player.weaponEquipied is null) { return; }

        if (_callback.started)
        {
            player.weaponEquipied.Shoot(transform);
            print($"{player.weaponEquipied.name} Shoot");
        }

        if (_callback.canceled)
        {
            player.weaponEquipied.ShootFinished();
            print($"{player.weaponEquipied.name} ShootFinished");
        }
    }
}
