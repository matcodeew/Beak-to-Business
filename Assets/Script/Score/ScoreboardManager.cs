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

    [Header("Needed data")]
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _scoreboardItemPrefab;

    private Dictionary<Player, ScoreboardItem> _scoreboardItem = new();
    private List<ScoreboardItem> _itemList;

    private int _maxPlayer = 10;

    #region Subscribe Event
    private void OnEnable()
    {
        EventManager.OnIncreaseScore += UpdatePlayerScore;
        EventManager.OnScoreChanged += SortScoreboardItem;
    }
    private void OnDisable()
    {
        EventManager.OnIncreaseScore -= UpdatePlayerScore;
        EventManager.OnScoreChanged -= SortScoreboardItem;
    }
    #endregion
    private void Start()
    {
        OnPlayerEnteredRoom(_player);
        OnPlayerEnteredRoom(_player2);
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
        ScoreboardItem newPrefab = Instantiate(_scoreboardItemPrefab, _container).GetComponent<ScoreboardItem>();
        newPrefab.Initialize(player);
        _scoreboardItem.Add(player, newPrefab);
    }

    public void RemoveScoreboardItem(Player player)
    {
        Destroy(_scoreboardItem[player].gameObject);
        _scoreboardItem.Remove(player);
    }
    public void SortScoreboardItem()
    {
        _itemList.Clear();
        foreach (var item in _scoreboardItem.Values)
        {
            _itemList.Add(item);
        }
        _itemList.Sort((a, b) => b.GetScoreText() - a.GetScoreText());
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

