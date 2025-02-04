using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GeneralBuff : InteractableObjects
{
    public float duration;
    public float value;
    public Stats stats;

    public override void PlayerInteract(Player player)
    {
        base.PlayerInteract(player);
        EventManager.ApplyBuff(stats, duration, value);
    }

}
