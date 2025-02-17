using UnityEngine.Events;
using UnityEngine;
using System;

public static class EventManager 
{
    public static event UnityAction OnScoreChanged;
    public static event UnityAction<Player, int> OnIncreaseScore;
    public static event Action<Stats, float, float, Player, Sprite> OnApplyBuff;
    
    public static event UnityAction OnSkinChanged;

    public static void UpdateScore()
    {
        OnScoreChanged.Invoke();
    }

    public static void IncreaseScore(Player player, int amount)
    {
        OnIncreaseScore.Invoke(player, amount);
    }

    public static void ApplyBuff(Stats _stat, float _duration, float _value, Player player, Sprite feedback)
    {
        OnApplyBuff.Invoke(_stat, _duration, _value, player, feedback);
    }
    
    public static void SetSkin()
    {
        OnSkinChanged.Invoke();
    }

}
