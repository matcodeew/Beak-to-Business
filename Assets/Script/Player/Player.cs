using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;

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
    private bool _canPickupWeapon = false;

    [Header("Player Skin")]
    [SerializeField] private Transform _skinParent;
    public NetworkVariable<int> SelectedSkinIndex = new NetworkVariable<int>(-1,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private GameObject _choosenSkin;

    [SerializeField] private Image _healthFill;
    [SerializeField] private SpriteRenderer _weaponRenderer;
    #endregion



    private void OnSkinChanged(int oldIndex, int newIndex)
    {
        ResetSkinVisibility();
        if (newIndex >= 0 && newIndex < _skinParent.childCount)
        {
            _skinParent.GetChild(newIndex).gameObject.SetActive(true);
            _choosenSkin = _skinParent.GetChild(newIndex).gameObject;
            GetComponent<PlayerMovement>().SetRightAnimator(_choosenSkin);

            EventManager.SetSkin();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        lifeText.text = _health.Value.ToString();
        _health.OnValueChanged += OnHealthChanged;
        SelectedSkinIndex.OnValueChanged += OnSkinChanged;
        if (IsOwner) SetSkin();

        OnHealthChanged(0f, stats.defaultHealth.Value);
        OnSkinChanged(0, SelectedSkinIndex.Value);
        SetPlayerAtRandomPosition();
        _canPickupWeapon = true;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        _health.OnValueChanged -= OnHealthChanged;

        if (weaponEquipied != null)
        {
            SpawnWeaponServerRpc(transform.position, GetComponent<PlayerDeath>()._weaponPrefabs.IndexOf(weaponEquipied.spawnableObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (!IsOwner) return;

        GetInteractibleObject(collision.gameObject);

        if (collision.CompareTag("Bullet") && collision.GetComponent<Bullet>().throwerID != OwnerClientId)
        {
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
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        
        #region Set Skin Verif

        if (playerMovement is null){
            throw new NullReferenceException("no component PlayerMovement on the player object");
        }
        if(SelectedSkinIndex.Value > 4 || SelectedSkinIndex.Value < -1) {

             throw new ArgumentOutOfRangeException(nameof(SelectedSkinIndex.Value), "Skin index must be between 0 and 4 includes");
        }
        
        #endregion
        
        ResetSkinVisibility();
        if (SelectedSkinIndex.Value == -1) { 
            SelectSkinServerRpc(PickRandomSkin()); 
        }

        SelectSkinServerRpc(SelectedSkinIndex.Value);

     
        //_choosenSkin = _skinParent.GetChild(SelectedSkinIndex.Value).gameObject;
        //_choosenSkin.SetActive(true);
        
        //playerMovement.SetRightAnimator(_choosenSkin);
        playerMovement.GetPlayerSpeed(stats.speed);

        
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
        if (interactibleObject.TryGetComponent(out InteractableObjects _interactibleObject))
        {
            if(interactibleObject.TryGetComponent<Interactible_Weapons>(out Interactible_Weapons w) && _canPickupWeapon)
            {
                if(weaponEquipied != null)
                {
                    int index = GetComponent<PlayerDeath>()._weaponPrefabs.IndexOf(weaponEquipied.spawnableObject);
                    SpawnWeaponServerRpc(transform.position, index);
                }
                Interact(interactibleObject, _interactibleObject);
                _canPickupWeapon = false;
                StartCoroutine(ReloadPickup());
                return;
            }
            else if(interactibleObject.TryGetComponent<Interactible_Weapons>(out Interactible_Weapons w1) && !_canPickupWeapon)
            {
                return; 
            }

            Interact(interactibleObject, _interactibleObject);
            
        }
    }

    private void Interact(GameObject obj, InteractableObjects interactibleObject)
    {
        interactibleObject.PlayerInteract(this);
        Server.instance.DestroyObjectOnServerRpc(obj.GetComponent<NetworkObject>().NetworkObjectId);
    }

    private IEnumerator ReloadPickup()
    {
        yield return new WaitForSeconds(2f);
        _canPickupWeapon = true;
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
        //_weaponRenderer.sprite = weaponEquipied.weaponImage;
        GetComponent<PlayerAudio>().PlayEquipedWeaponAudio();
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
    public void HealServerRpc(float value)
    {
        _health.Value += value;
        if (_health.Value >= stats.defaultHealth.Value) _health.Value = stats.defaultHealth.Value;
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
        _healthFill.fillAmount = newValue / stats.defaultHealth.Value;
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
        GameObject bullet = Instantiate(weaponEquipied.weaponData.bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.GetComponent<Bullet>().InitializeBulletClientRpc(bulletSpeed, fireRange, direction, throwerID, damages);
    }
    #endregion
}