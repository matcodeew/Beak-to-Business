using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreboardManager : NetworkBehaviour
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
        

        AddScoreboardItemServerRpc(connectionID);
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
    public void AddScoreboardItemServerRpc(ulong connectionID)
    {
        Debug.Log("---------- 1 ----------");
        Player player = null;
        Debug.Log("---------- 2 ----------");

        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(connectionID))
        {
            NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[connectionID].PlayerObject;
            player = networkObject.gameObject.GetComponent<Player>();
        }
        Debug.Log("---------- 3 ----------");

        GameObject newPrefab = Instantiate(_scoreboardItemPrefab, _container);
        NetworkObject newNetworkObject = newPrefab.GetComponent<NetworkObject>();

        Debug.Log("---------- 4 ----------");


        newNetworkObject.Spawn();
        newNetworkObject.TrySetParent(_container, true);

        Debug.Log("---------- 5 ----------");


        newPrefab.GetComponent<ScoreboardItem>().SetPlayer(connectionID);
        _scoreboardItem.Add(player, newPrefab.GetComponent<ScoreboardItem>());

        Debug.Log("---------- 6 ----------");


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

