using UnityEngine;

public class Interactible_Weapons : InteractableObjects
{
    [SerializeField] private GameObject _weaponPrefab;
    public override void PlayerInteract(Player player)
    {
        base.PlayerInteract(player);
        Weapon _weapon = _weaponPrefab.GetComponent<Weapon>();
        player.SetWeapon(_weapon, _weapon.weaponData);
    }
}
