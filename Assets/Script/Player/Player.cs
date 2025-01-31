using UnityEngine;


[System.Serializable]
public struct PlayerStats
{
    public float speed;
    public float health;
    public int XP;
    public int score;
}
public class Player : MonoBehaviour
{
    public PlayerStats stats;
    public Weapon weaponEquipied;
    private void OnTriggerEnter(Collider collider)
    {
        GetInteractibleObject(collider);
    }

    private void GetInteractibleObject(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out InteractableObjects _interactibleObject))
        {
            _interactibleObject.PlayerInteract(this);
            Destroy(collider.gameObject);
        }
    }

    public void TakeDamage(float _damage, Player _playerGivenDamage)
    {
        stats.health = Mathf.Clamp(stats.health - _damage, 0, stats.health);

        if (stats.health <= 0)
        {
            Die(_playerGivenDamage);
        }
    }

    private void Die(Player _playerGivenDamage)
    {
        Destroy(gameObject); //mettre returnToPool a la place.

        //return to main menus.
        //add le score au joueur qui a tuer
        //drop l'arme equiper
    }

    public void SetWeapon(Weapon weapon, WeaponStats data)
    {
        weaponEquipied = weapon;
        weaponEquipied.Initialize(data);
    }
}

