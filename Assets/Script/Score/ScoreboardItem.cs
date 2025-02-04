using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor.MemoryProfiler;
using UnityEngine.Scripting;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _position;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerScore;

    public void UpdateScore(Player player)
    {

        _playerScore.text = player.stats.score.Value.ToString();
        EventManager.UpdateScore();
    }

    public void Initialize(Player player)
    {
        _playerName.text = player.gameObject.name;
        UpdateScore(player);
    }

    public int GetScoreText() => int.Parse(_playerScore.text);

    public void UpdatePlayerPosition(int pos)
    {
        _position.text = pos.ToString() + "ème";
    }
    

}
