using System.Collections.Generic;
using UnityEngine;

public struct BasePlayerStats
{
    public float baseDamage;
    public float baseSpeed;
    public float baseFireRate;
}
public enum Stats
{
    NONE,
    health,
    damage,
    speed,
    fireRate
}
public class BuffManager : MonoBehaviour
{
    private static Dictionary<Stats, string> _activeBonus = new();
    private BasePlayerStats _baseStats;

    private PlayerStats _playerStats;
    private WeaponsStats _weaponStats;

    void Awake()
    {
        EventManager.OnApplyBuff += ApplyBuff;
        SetBonusDictionaty();

    }
    private void SetBonusDictionaty()
    {
        _activeBonus.Add(Stats.health, string.Empty);
        _activeBonus.Add(Stats.damage, string.Empty);
        _activeBonus.Add(Stats.speed, string.Empty);
        _activeBonus.Add(Stats.fireRate, string.Empty);
    }

    private void SetBaseStat(Player player)
    {
        if (_activeBonus[Stats.speed] == string.Empty)
        {
            print($"set base Speed {player.stats.speed} ");
            _baseStats.baseSpeed = player.stats.speed;
        }
        if (_activeBonus[Stats.damage] == string.Empty && player.weaponEquipied is not null)
        {
            print($"set base Damage {player.weaponEquipied.stats.damage} ");
            _baseStats.baseDamage = player.weaponEquipied.stats.damage;
        }
        _playerStats = player.stats;
        if (player.weaponEquipied != null)
        {
            _weaponStats = player.weaponEquipied.stats;
        }
    }
    public void ApplyBuff(Stats _stat, float _duration, float _value, Player player, Sprite sprite)
    {
        SetBaseStat(player);

        if (_activeBonus[_stat] == string.Empty)
        {
            switch (_stat)
            {
                case Stats.health:
                    player.HealServerRpc(player.stats.defaultHealth.Value * _value);
                    //GetComponent<PlayerAudio>().PlayHealAudio();
                    break;

                case Stats.damage:
                    _weaponStats.damage *= _value;
                    break;

                case Stats.speed:
                    _playerStats.speed *= _value;
                    break;

                case Stats.fireRate:
                    _weaponStats.fireRate *= _value;
                    //GetComponent<PlayerAudio>().PlayPimentAudio();
                    break;

                case Stats.NONE:
                    break;
            }
        }
        SetStatsToPlayer(player);

        _activeBonus[_stat] = TimerManager.StartTimer(_duration, () =>
             {
                 switch (_stat)
                 {
                     case Stats.health:
                         // _playerStats.health = _baseStats.baseHealth;
                         break;

                     case Stats.damage:
                         _weaponStats.damage = _baseStats.baseDamage;
                         break;

                     case Stats.speed:
                         _playerStats.speed = _baseStats.baseSpeed;
                         break;

                     case Stats.fireRate:
                         _weaponStats.fireRate = _baseStats.baseFireRate;
                         break;

                     case Stats.NONE:
                         break;
                 }
                 SetStatsToPlayer(player);
                 _activeBonus[_stat] = string.Empty;
             });
    }
    private void SetStatsToPlayer(Player player)
    {
        if (player.weaponEquipied is not null)
        {
            player.weaponEquipied.stats = _weaponStats;
        }
        player.stats = _playerStats;
        player.SetPlayerMovementSpeed(player.GetComponent<PlayerMovement>());
    }
}