using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private GameObject objectToSpawn;


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
            //TestServerRpc(transform.position + new Vector3(0, 2,0));    
            //print($"{player.weaponEquipied.name} Shoot");
        }

        if (_callback.canceled)
        {
            player.weaponEquipied.ShootFinished();
            //print($"{player.weaponEquipied.name} ShootFinished");
        }
    }

}
