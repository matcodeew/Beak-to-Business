using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    public float duration;
    public float value;

    [ContextMenu("OnPickedUp")]
    public void OnPickedUp()
    {
        Buff.ApplyBuff(Stats.speed, duration, value);
    }
}
