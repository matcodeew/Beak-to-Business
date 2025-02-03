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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetInteractibleObject(collision.gameObject);
        if(collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            TakeDamage(bullet.playerLuncher.weaponEquipied.stats.damage, bullet.playerLuncher);
            Destroy(collision.gameObject);
        }
    }
    private void Update()
    {
        if (weaponEquipied is null) return;
        weaponEquipied.ShootHandler(Time.deltaTime);
    }

    private void GetInteractibleObject(GameObject interactibleObject)
    {
        if (interactibleObject.TryGetComponent(out InteractableObjects _interactibleObject))
        {
            _interactibleObject.PlayerInteract(this);
            Destroy(interactibleObject);
            EventManager.IncreaseScore(this, 10);
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

