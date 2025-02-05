using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

[System.Serializable]
public struct PlayerStats
{
    public float speed;
    public NetworkVariable<float> health;
    public NetworkVariable<int> XP;
    public NetworkVariable<int> score;
}
public class Player : NetworkBehaviour
{
    [Header("Player stats")]
    public PlayerStats stats;
    public Weapon weaponEquipied;

    [SerializeField] private GameObject _bulletPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;

        GetInteractibleObject(collision.gameObject);

        if(collision.CompareTag("Bullet") && collision.transform != transform)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            Player bulletFiried = collision.GetComponent<Bullet>().playerLuncher;
            TakeDamage(bulletFiried.weaponEquipied.stats.damage, bulletFiried);
            Server.instance.DestroyObjectOnServerRpc(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
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
            Server.instance.DestroyObjectOnServerRpc(interactibleObject.GetComponent<NetworkObject>().NetworkObjectId);
            //EventManager.IncreaseScore(this, 10);
            GetComponent<PlayerData>().IncreaseScoreServerRpc(10);
        }
    }

    public void TakeDamage(float _damage, Player _playerGivenDamage)
    {
        stats.health.Value = Mathf.Clamp(stats.health.Value - _damage, 0, stats.health.Value);

        if (stats.health.Value <= 0)
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

    public void Shoot()
    {
        if (weaponEquipied != null)
        {
            weaponEquipied.Shoot(this.transform);
        }
    }

    

    public void RequestSpawnBullet(Vector2 spawnPosition, Vector2 direction)
    {
        Debug.Log("Request To spawn bullet");

        if (IsOwner)
        {
            Debug.Log($"Calling SpawnBulletServerRpc from Client ID: {NetworkManager.Singleton.LocalClientId}");
            SpawnBulletServerRpc(spawnPosition, direction);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRpc(Vector2 spawnPosition, Vector2 direction)
    {
        Debug.Log($"SpawnBulletServerRpc triggered on Server! Sender: {OwnerClientId}");
        GameObject bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.GetComponent<Bullet>().InitializeBullet(spawnPosition, direction, weaponEquipied.stats.bulletSpeed, this);
    }

}

