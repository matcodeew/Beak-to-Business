using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    public float duration;
    public float value;

    public void OnPickedUp()
    {
        Buff.ApplyBuff(Stats.speed, duration, value);
    }
}
