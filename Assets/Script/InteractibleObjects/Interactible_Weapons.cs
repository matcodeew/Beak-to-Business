using System;
using UnityEngine;

public class Interactible_Weapons : InteractableObjects
{
    [SerializeField] private GameObject _weaponPrefab;

    private void Awake() // mettre quand l'objet est spawn sur la map avec Network 
    {
        GetComponent<SpriteRenderer>().sprite = _weaponPrefab.GetComponent<SpriteRenderer>().sprite;
        name = _weaponPrefab.name;
    }

    public override void PlayerInteract(Player player)
    {
        base.PlayerInteract(player);
        Weapon _weapon = _weaponPrefab.GetComponent<Weapon>();
        player.SetWeapon(_weapon, _weapon.weaponData);
    }
}
