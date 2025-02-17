using UnityEngine;

public class GeneralBuff : InteractableObjects
{
    [Header("stats that will be buff / debuff")]
    [SerializeField] private Stats _stats;
    [Header("Value of effect")]
    [SerializeField] private float _value;
    [Header("Duration of effect")]
    [SerializeField] private float _duration;
    [Header("Feedback of buff / debuff")]
    [SerializeField] private Sprite _feedback;
    public override void PlayerInteract(Player player)
    {
        base.PlayerInteract(player);
        EventManager.ApplyBuff(_stats, _duration, _value, player, _feedback);
    }
}