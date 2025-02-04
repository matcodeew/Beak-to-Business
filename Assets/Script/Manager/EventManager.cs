using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    public static event UnityAction OnScoreChanged;
    public static event UnityAction<Player, int> OnIncreaseScore;

    public static void UpdateScore()
    {
        OnScoreChanged.Invoke();
    }

    public static void IncreaseScore(Player player, int amount)
    {
        OnIncreaseScore.Invoke(player, amount);
    }
}
