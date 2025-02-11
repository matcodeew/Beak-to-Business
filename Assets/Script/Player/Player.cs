using TMPro;
using Unity.Netcode;
using System;
using System.Collections;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.VisualScripting;
using UnityEditor;

[System.Serializable]
public struct PlayerStats
{
    public float speed;
    public NetworkVariable<float> defaultHealth;
    public NetworkVariable<int> XP;
}
public class Player : NetworkBehaviour
{
    #region Variables
    [Header("Player Stats")]
    public PlayerStats stats;
    public Weapon weaponEquipied;

    [Header("Combat & Shooting")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Player Health")]
    public TextMeshProUGUI lifeText;
    private NetworkVariable<float> _health = new NetworkVariable<float>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [Header("Player Abilities & Interaction")]
    private bool _canPickupObject = false;

    [Header("Player Skin")]
    [SerializeField] private Transform _skinParent;
    public NetworkVariable<int> SelectedSkinIndex = new NetworkVariable<int>(-1, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private GameObject _choosenSkin;
    #endregion

    private void OnSkinChanged(int oldIndex, int newIndex)
    {
        ResetSkinVisibility();
        if (newIndex >= 0 && newIndex < _skinParent.childCount)
        {
            _skinParent.GetChild(newIndex).gameObject.SetActive(true);
            _choosenSkin = _skinParent.GetChild(newIndex).gameObject;
            GetComponent<PlayerMovement>().SetRightAnimator(_choosenSkin);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        lifeText.text = _health.Value.ToString();
        _health.OnValueChanged += OnHealthChanged;
        SelectedSkinIndex.OnValueChanged += OnSkinChanged;
        if(IsOwner) SetSkin();

        OnHealthChanged(0f, stats.defaultHealth.Value);
        OnSkinChanged(0, SelectedSkinIndex.Value);
        SetPlayerAtRandomPosition();
        _canPickupObject = true;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        _health.OnValueChanged -= OnHealthChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;

        GetInteractibleObject(collision.gameObject);

        if (collision.CompareTag("Bullet") && collision.GetComponent<Bullet>().throwerID != OwnerClientId)
        {
            Debug.Log("BUllet");
            Bullet bullet = collision.GetComponent<Bullet>();
            TakeDamage(bullet.Damages, bullet.throwerID);
            Server.instance.DestroyObjectOnServerRpc(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
    private void Update()
    {

        if (weaponEquipied is null) return;
        weaponEquipied.ShootHandler(Time.deltaTime);
    }

    #region Player Skin Management
    public void SetSkinIndex(int value) => SelectSkinServerRpc(value); // Call this to set the skin index.
    private int PickRandomSkin() => UnityEngine.Random.Range(0, 5); //pick random index between 0 and 4.
    
    [ContextMenu("SetSkin")]
    private void SetSkin() //playerSkinIndex must be between 0 and 4 includes.
    {
        if(SelectedSkinIndex.Value > 4 || SelectedSkinIndex.Value < -1) {
            throw new ArgumentOutOfRangeException(nameof(SelectedSkinIndex.Value), "Skin index must be between 0 and 4 includes");
        }
        
        ResetSkinVisibility();
        if (SelectedSkinIndex.Value == -1) { 
            SelectSkinServerRpc(PickRandomSkin()); 
        }

        SelectSkinServerRpc(SelectedSkinIndex.Value);

        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectSkinServerRpc(int index)
    {
        if (index >= 0 && index < _skinParent.childCount)
        {
            SelectedSkinIndex.Value = index;
        }
    }



    private void ResetSkinVisibility()
    {
        for(int i = 0 ; i < _skinParent.childCount; i++)
        {
            _skinParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Player Interaction
    private void GetInteractibleObject(GameObject interactibleObject)
    {
        if (_canPickupObject && interactibleObject.TryGetComponent(out InteractableObjects _interactibleObject))
        {
            if (weaponEquipied != null)
            {
                int index = GetComponent<PlayerDeath>()._weaponPrefabs.IndexOf(weaponEquipied.spawnableObject);
                SpawnWeaponServerRpc(transform.position, index);
            }

            _interactibleObject.PlayerInteract(this);
            Server.instance.DestroyObjectOnServerRpc(interactibleObject.GetComponent<NetworkObject>().NetworkObjectId);
            _canPickupObject = false;
            StartCoroutine(ReloadPickup());
        }
    }

    private IEnumerator ReloadPickup()
    {
        yield return new WaitForSeconds(2f);
        _canPickupObject = true;
    }
    #endregion

    #region Weapon Management
    [ServerRpc(RequireOwnership = false)]
    public void SpawnWeaponServerRpc(Vector3 spawnPosition, int weaponIndex)
    {
        GameObject weapon = Instantiate(GetComponent<PlayerDeath>()._weaponPrefabs[weaponIndex], transform.position, Quaternion.identity);
        weapon.GetComponent<NetworkObject>().Spawn();
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
    #endregion

    #region Player Positioning
    public void SetPlayerAtRandomPosition()
    {
        Rect zone = Server.instance.spawnZone;
        Vector2 position = new Vector2(UnityEngine.Random.Range(zone.xMin, zone.xMax), UnityEngine.Random.Range(zone.yMin, zone.yMax));
        transform.position = position;
    }
    #endregion

    #region Health Management
    public void TakeDamage(float damage, ulong throwerID = 0)
    {
        float copy = _health.Value - damage;
        TakeDamageServerRpc(damage);
        if (copy <= 0)
        {
            SetHealthValueServerRpc(0);
            GetComponent<PlayerDeath>().Die(throwerID);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(float damage)
    {
        _health.Value -= damage;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetHealthValueServerRpc(float value)
    {
       _health.Value = value;
    }

    private void OnHealthChanged(float previousValue, float newValue)
    {
        lifeText.text = newValue.ToString();
    }
    #endregion

    #region Bullet Management
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
    #endregion
}