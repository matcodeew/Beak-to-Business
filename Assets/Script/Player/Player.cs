using Newtonsoft.Json.Bson;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerStats
{
    public float speed;
    public NetworkVariable<float> defaultHealth;
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
    //private NetworkVariable<float> health;
    private NetworkVariable<float> health = new NetworkVariable<float>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);



    [Header("Death")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private PlayerInput _playerInputs;
    [SerializeField] private GameObject _deathUI;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _quitButton;
    private PlayerData _playerData;

    NetworkVariable<bool> _isDead = new NetworkVariable<bool>(false);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        _playerData = GetComponent<PlayerData>();

        lifeText.text = health.Value.ToString();

        health.OnValueChanged += OnHealthChanged;

        OnHealthChanged(0f, stats.defaultHealth.Value);

        _isDead.OnValueChanged += OnDeathStatusChanged;
        if (_isDead.Value)
            OnDeathStatusChanged(false, _isDead.Value);

    }

    void OnDeathStatusChanged(bool previousValue, bool newValue)
    {
        _spriteRenderer.enabled = !newValue;
        _collider.enabled = !newValue;
        _healthBar.SetActive(!newValue);
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
        {
            TakeDamage(50, OwnerClientId);
        }


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
        //NetworkManager.Singleton.ConnectedClients[throwerID].PlayerObject.GetComponent<PlayerData>().IncreaseScoreServerRpc(20);

        if (copy <= 0)
        {
            SetHealthValueServerRpc(0);
            Die(throwerID);
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(float damage)
    {
        health.Value -= damage;
    }

    [ServerRpc]
    private void SetHealthValueServerRpc(float value)
    {
        health.Value = value;
    }

    private void OnHealthChanged(float previousValue, float newValue)
    {
        lifeText.text = newValue.ToString();
    }

    private void Die(ulong throwerID)
    {
        NetworkManager.Singleton.ConnectedClients[throwerID].PlayerObject.GetComponent<PlayerData>().IncreaseScoreServerRpc(100);

        DieServerRpc();

        //_spriteRenderer.enabled = false;
        //_collider.enabled = false;
        //_healthBar.SetActive(false);
        _playerInputs.enabled = false;
        _deathUI.SetActive(true);
        _scoreText.text = _playerData.Score.Value.ToString();
        _playerData.SetScoreServerRpc(0);

        //Désactiver sprite - SERVEUR 
        //Désactiver collider - SERVEUR
        //Désactiver barre de vie / vie - SERVEUR
        //Désactiver Inputs - LOCAL
        //Poser l'arme - SERVER
        //Afficher UI - LOCAL
        //Score
        //Play Again
        //Quit -> Redirige vers le site
        //Score à 0 - SERVEUR
        //Supprimer du scoreboard - SERVEUR
    }

    [ServerRpc]
    private void DieServerRpc()
    {
        _isDead.Value = true;
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

