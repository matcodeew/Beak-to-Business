using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Unity;
using UnityEngine.Events;

public class ScoreboardManager : MonoBehaviour
{
    [Header("TEST")]
    [SerializeField] private Player _player;
    [SerializeField] private Player _player2;
    [Space(5)]
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _scoreboardItemPrefab;
    [SerializeField] private Dictionary<Player, ScoreboardItem> _scoreboardItem = new();
    [SerializeField] private List<ScoreboardItem> _itemList;

    private void Awake()
    {
        EventManager.OnIncreaseScore += UpdatePlayerScore;
        EventManager.OnScoreChanged += SortScoreboardItem;
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
        SortScoreboardItem();
    }

    public void OnPlayerLeavedRoom(Player playerLeaved)
    {
        RemoveScoreboardItem(playerLeaved);
    }

    public void AddScoreboardItem(Player player)
    {
        GameObject newPrefab = Instantiate(_scoreboardItemPrefab, _container);
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

    private void Start()
    {
        OnPlayerEnteredRoom(_player);
        OnPlayerEnteredRoom(_player2);
    }

    public void UpdatePlayerScore(Player player, int amount)
    {
        _scoreboardItem[player].UpdateScore(player);
    }
}

