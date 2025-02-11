using UnityEngine;

public class GeneralBuff : InteractableObjects
{
    [Header("stats that will be buff / debuff")]
    [SerializeField] private Stats _stats;
    [Header("Value of effect")]
    [SerializeField] private float _value;
    [Header("Duration of effect")]
    [SerializeField] private float _duration;

    public override void PlayerInteract(Player player)
    {
        base.PlayerInteract(player);
        EventManager.ApplyBuff(_stats, _duration, _value, player);
    }
}