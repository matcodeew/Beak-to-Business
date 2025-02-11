using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    public static event UnityAction OnScoreChanged;
    public static event UnityAction<Player, int> OnIncreaseScore;
    public static event UnityAction<Stats, float, float, Player> OnApplyBuff;
    
    public static event UnityAction OnSkinChanged;

    public static void UpdateScore()
    {
        OnScoreChanged.Invoke();
    }

    public static void IncreaseScore(Player player, int amount)
    {
        OnIncreaseScore.Invoke(player, amount);
    }

    public static void ApplyBuff(Stats _stat, float _duration, float _value, Player player)
    {
        OnApplyBuff.Invoke(_stat, _duration, _value, player);
    }
    
    public static void SetSkin()
    {
        OnSkinChanged.Invoke();
    }

}
