using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        EventManager.OnIncreaseScore += IncreaseScore;
    }

    public void UpdateScore(Player _player)
    {
        //scoreText.text = _player.stats.score.ToString();
    }

    public void IncreaseScore(Player _player, int _amount)
    {
        //_player.stats.score.Value += _amount;
        UpdateScore(_player);
    }
}
