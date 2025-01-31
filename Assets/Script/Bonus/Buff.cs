using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

struct baseStats
{
    public int baseHealth;
    public int baseDamage;
    public int baseSpeed;
    public int baseFireRate;
}

public class Buff : MonoBehaviour
{
    public static Dictionary<Stats, float> _statsDico = new();
    
    //public PlayerStats playerStats;

    void Start()
    {
        _statsDico.Add(Stats.health, 0);
        _statsDico.Add(Stats.damage, 0);
        _statsDico.Add(Stats.speed, 0);
        _statsDico.Add(Stats.fireRate, 0);
    }

    void Update()
    {
        foreach (var key in _statsDico.Keys)
        {
            if (_statsDico[key] >= 0)
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
            case Stats.health :
                Debug.Log("health");
                //playerStats.health *= baseStats.baseHealth;
                //playerStats.health *= _value;
                break;
            case Stats.damage :
                Debug.Log("damage");
                //playerStats.health *= baseStats.baseDamage;
                //playerStats.damage *= _value;
                break;
            case Stats.speed :
                Debug.Log("speed");
                //playerStats.health *= baseStats.baseSpeed;
                //playerStats.speed *= _value;
                break;
            case Stats.fireRate :
                Debug.Log("fireRate");
                //playerStats.health *= baseStats.baseFireRate;
                //playerStats.fireRate *= _value;
                break;
        }
        
        _statsDico[_stat] = _duration;
    }
}

public enum Stats
{
    health,
    damage,
    speed,
    fireRate
}