using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    

    public void IncreaseScore(Player _player, int _amount)
    {
        _player.score += _amount;
        UpdateScore(_player);
    }

    public void UpdateScore(Player _player)
    {
        scoreText.text = _player.score.ToString();
    }
}
