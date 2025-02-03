using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

struct baseStats
{
    public int baseHealth;
    public int baseDamage;
    public int baseSpeed;
    public int baseFireRate;
}

public enum Stats
{
    health,
    damage,
    speed,
    fireRate
}
public class Buff : MonoBehaviour
{
    public static Dictionary<Stats, float> _statsDico = new();

    public PlayerStats playerStats;

    void Awake()
    {
        _statsDico.Add(Stats.health, 0);
        _statsDico.Add(Stats.damage, 0);
        _statsDico.Add(Stats.speed, 0);
        _statsDico.Add(Stats.fireRate, 0);
    }

    void Update()
    {
        foreach (var key in _statsDico.Keys.ToList())
        {
            if (_statsDico[key] > 0)
            {
                _statsDico[key] -= Time.deltaTime;

            }

            if (_statsDico[key] <= 0)
            {
                switch (key)
                {
                    case Stats.health:
                        //playerStats.health = baseStats.baseHealth;
                        break;
                    case Stats.damage:
                        //playerStats.damage = baseStats.baseDamage;
                        break;
                    case Stats.speed:
                        //playerStats.speed = baseStats.baseSpeed;
                        Debug.Log($"Buff {key.ToString()} finished");
                        break;
                    case Stats.fireRate:
                        //playerStats.fireRate = baseStats.baseFireRate;
                        break;
                }
            }
        }
    }

    public static void ApplyBuff(Stats _stat, float _duration, float _value)
    {
        switch (_stat)
        {
            case Stats.health:
                //playerStats.health *= baseStats.baseHealth;
                //playerStats.health *= _value;
                break;
            case Stats.damage:
                //playerStats.health *= baseStats.baseDamage;
                //playerStats.damage *= _value;
                break;
            case Stats.speed:
                //playerStats.health *= baseStats.baseSpeed;
                //playerStats.speed *= _value;
                break;
            case Stats.fireRate:
                //playerStats.health *= baseStats.baseFireRate;
                //playerStats.fireRate *= _value;
                break;
        }
        Debug.Log($"Apply {_stat.ToString()} buff for {_duration} and value is : {_value}");
        _statsDico[_stat] = _duration;
    }
}