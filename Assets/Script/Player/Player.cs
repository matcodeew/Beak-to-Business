using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public TextMeshProUGUI lifeText;
    public NetworkVariable<float> health;

    [HideInInspector] public NetworkVariable<bool> isDead;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        lifeText.text = health.Value.ToString();
        health.OnValueChanged += OnHealthChanged;
        OnHealthChanged(0f, health.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        health.OnValueChanged -= OnHealthChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;

        GetInteractibleObject(collision.gameObject);

        if(collision.CompareTag("Bullet") && collision.GetComponent<Bullet>().throwerID != OwnerClientId)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            TakeDamage(bullet.Damages, bullet.throwerID);
            Server.instance.DestroyObjectOnServerRpc(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(50, OwnerClientId);


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


    public void TakeDamage(float damage, ulong throwerID = 0)
    {
        float copy = health.Value - damage;
        TakeDamageServerRpc(damage);
        NetworkManager.Singleton.ConnectedClients[throwerID].PlayerObject.GetComponent<PlayerData>().IncreaseScoreServerRpc(20);

        if (copy <= 0)
        {
            Die(throwerID);
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(float damage)
    {
        health.Value -= damage;
    }

    private void OnHealthChanged(float previousValue, float newValue)
    {
        lifeText.text = newValue.ToString();
    }


    private void Die(ulong throwerID)
    {
        NetworkManager.Singleton.ConnectedClients[throwerID].PlayerObject.GetComponent<PlayerData>().IncreaseScoreServerRpc(100);
        //isDead.Value = true;

        //GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<BoxCollider2D>().enabled = false;



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
        if (IsOwner)
        {
            SpawnBulletServerRpc(spawnPosition, 
                weaponEquipied.stats.bulletSpeed, 
                weaponEquipied.stats.fireRange, 
                direction, 
                OwnerClientId,
                weaponEquipied.stats.damage);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRpc(Vector2 spawnPosition, float bulletSpeed, float fireRange, Vector2 direction, ulong throwerID, float damages)
    {
        GameObject bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.GetComponent<Bullet>().InitializeBulletClientRpc(bulletSpeed, fireRange, direction, throwerID, damages);
    }

}

