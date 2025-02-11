using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreboardItem : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _position;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerScore;

    Player _connectedPlayer;

    public void UpdateScore(Player player)
    {

        //_playerScore.text = player.stats.score.Value.ToString();
        EventManager.UpdateScore();
    }

    public void SetPlayer(ulong connectionID)
    {
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(connectionID))
        {
            NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[connectionID].PlayerObject;
            _connectedPlayer = networkObject.gameObject.GetComponent<Player>();
        }
    }

    private void Initialize()
    {
        _playerName.text = _connectedPlayer.gameObject.name;
        UpdateScore(_connectedPlayer);
    }

    public int GetScoreText() => int.Parse(_playerScore.text);

    public void UpdatePlayerPosition(int pos)
    {
        _position.text = pos.ToString() + "ème";
    }
    

}
