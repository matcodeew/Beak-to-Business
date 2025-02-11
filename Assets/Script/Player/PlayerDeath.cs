using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class PlayerDeath : NetworkBehaviour
{
    [Header("Death")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private PlayerInput _playerInputs;
    [SerializeField] private GameObject _deathUI;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _respawnTimerText;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _quitButton;
    private PlayerData _playerData;

    public NetworkVariable<bool> _isDead = new NetworkVariable<bool>(false);

    public List<GameObject> _weaponPrefabs = new List<GameObject>();

    private bool _isRespawning = false;
    [SerializeField] private float _respawnTime = 2f;
    private float _currentTimer = 0f;

    private Player _player;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        _playerData = GetComponent<PlayerData>();
        _player = GetComponent<Player>();
        _playAgainButton.onClick.AddListener(PlayAgain);

        _isDead.OnValueChanged += OnDeathStatusChanged;
        if (_isDead.Value)
            OnDeathStatusChanged(false, _isDead.Value);
    }

    #region Unity Functions

    private void Update()
    {
        if(_isRespawning)
        {
            if(_currentTimer >= 0)
            {
                _currentTimer -= Time.deltaTime;
                _respawnTimerText.text = ((int)_currentTimer).ToString();
            }
            else
            {
                _isRespawning = false;
                _respawnTimerText.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Event Functions

    private void OnDeathStatusChanged(bool previousValue, bool newValue)
    {
        _spriteRenderer.enabled = !newValue;
        _collider.enabled = !newValue;
        _healthBar.SetActive(!newValue);
    }

    #endregion

    public void Die(ulong throwerID)
    {
        NetworkManager.Singleton.ConnectedClients[throwerID].PlayerObject.GetComponent<PlayerData>().IncreaseScoreServerRpc(100);

        DropGunOnMap();
        DieServerRpc(true);

        _playerInputs.enabled = false;
        _deathUI.SetActive(true);
        _scoreText.text = _playerData.Score.Value.ToString();
        _playerData.SetScoreServerRpc(0);

        //Play Again
        //Quit -> Redirige vers le site
        //Supprimer du scoreboard - SERVEUR
    }

    public void PlayAgain()
    {
        _player.SetPlayerAtRandomPosition();
        _isRespawning = true;
        _respawnTimerText.gameObject.SetActive(true);
        _currentTimer = _respawnTime;

        Invoke("PerformRespawn", _respawnTime);
    }

    private void PerformRespawn()
    {
        _playerInputs.enabled = true;
        _deathUI.SetActive(false);
        _player.SetHealthValueServerRpc(_player.stats.defaultHealth.Value);

        DieServerRpc(false);
    }


    private void DropGunOnMap()
    {
        if (_player.weaponEquipied == null) return;

        GameObject weapon = _player.weaponEquipied.spawnableObject;
        _player.weaponEquipied = null;


        int prefabIndex = _weaponPrefabs.IndexOf(weapon);
        //int prefabIndex = _weaponPrefabs.FindIndex(obj => obj.name == weapon.name);

        SpawnGunOnServerRpc(prefabIndex, transform.position);
    }



    #region RPC Functions

    [ServerRpc(RequireOwnership = false)]
    private void SpawnGunOnServerRpc(int prefabIndex, Vector3 position)
    {
        GameObject weapon = Instantiate(_weaponPrefabs[prefabIndex], position, Quaternion.identity);
        weapon.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRpc(bool value)
    {
        _isDead.Value = value;
    }

    #endregion

}


