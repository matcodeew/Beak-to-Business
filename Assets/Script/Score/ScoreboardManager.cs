using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager instance;

    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _scoreboardItemPrefab;
    [SerializeField] private Dictionary<Player, ScoreboardItem> _scoreboardItem = new();
    [SerializeField] private List<ScoreboardItem> _itemList;

    private int _maxPlayer = 10;

    private void Awake()
    {
        instance = this;

        EventManager.OnIncreaseScore += UpdatePlayerScore;
        EventManager.OnScoreChanged += SortScoreboardItem;
    }

    public void OnPlayerEnteredRoom(ulong connectionID)
    {
        

        AddScoreboardItem(connectionID);
        SortScoreboardItem();
    }

    public void OnPlayerLeavedRoom(ulong connectionID)
    {
        Player playerLeaved = null;

        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(connectionID))
        {
            var networkObject = NetworkManager.Singleton.ConnectedClients[connectionID].PlayerObject;
            playerLeaved = networkObject.GetComponent<Player>();
        }
        else
        {
            Debug.Log($"{connectionID} not contained in Connection Client list");
        }

        if (playerLeaved == null)
        {
            Debug.Log("Player not found");
            return;
        }

        RemoveScoreboardItem(playerLeaved);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreboardItem(ulong connectionID)
    {
        Player player = null;

        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(connectionID))
        {
            NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[connectionID].PlayerObject;
            player = networkObject.gameObject.GetComponent<Player>();
        }

        if (player == null)
        {
            Debug.Log("Player not found");
            return;
        }

        Debug.Log("Create object");

        GameObject newPrefab = Instantiate(_scoreboardItemPrefab, _container);
        NetworkObject newNetworkObject = newPrefab.GetComponent<NetworkObject>();

        newNetworkObject.Spawn();
        newNetworkObject.TrySetParent(_container, true);

        newPrefab.GetComponent<ScoreboardItem>().Initialize(player);
        _scoreboardItem.Add(player, newPrefab.GetComponent<ScoreboardItem>());
    }

    public void RemoveScoreboardItem(Player player)
    {
        Destroy(_scoreboardItem[player].gameObject);
        _scoreboardItem.Remove(player);
    }

    [ContextMenu("SortScoreboardItem")]
    public void SortScoreboardItem()
    {
        _itemList.Clear();
        foreach (var item in _scoreboardItem.Values)
        {
            _itemList.Add(item);
        }
        _itemList.Sort((a, b) => (b.GetScoreText() - (a.GetScoreText())));
        for (int i = 0; i < _itemList.Count; i++)
        {
            _itemList[i].transform.SetSiblingIndex(i);
            _itemList[i].UpdatePlayerPosition(i + 1);
        }
    }


    public void UpdatePlayerScore(Player player, int amount)
    {
        _scoreboardItem[player].UpdateScore(player);
    }
}

