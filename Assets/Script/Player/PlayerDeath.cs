using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDeath : NetworkBehaviour
{
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

    #region Netcode Functions

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _playerData = GetComponent<PlayerData>();
        _playAgainButton.onClick.AddListener(PlayAgain);

        _isDead.OnValueChanged += OnDeathStatusChanged;
        if (_isDead.Value)
            OnDeathStatusChanged(false, _isDead.Value);
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

        DieServerRpc(true);

        _playerInputs.enabled = false;
        _deathUI.SetActive(true);
        _scoreText.text = _playerData.Score.Value.ToString();
        _playerData.SetScoreServerRpc(0);

        //Poser l'arme - SERVER
        //Play Again
        //Quit -> Redirige vers le site
        //Supprimer du scoreboard - SERVEUR
    }

    public void PlayAgain()
    {
        _playerInputs.enabled = true;
        _deathUI.SetActive(false);

        Player player = GetComponent<Player>();

        player.SetHealthValueServerRpc(player.stats.defaultHealth.Value);

        DieServerRpc(false);
    }


    #region RPC Functions

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRpc(bool value)
    {
        _isDead.Value = value;
    }

    #endregion

}


