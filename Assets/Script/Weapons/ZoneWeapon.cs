using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : RangeTravelWeapon
{
    [SerializeField] private Collider2D _zoneCollider;
    private List<Collider2D> _enemiesInZone = new List<Collider2D>();
    private bool _isShooting = false;
    private Player _playerGivenDamage;

    public override void Shoot(Transform playerTransform)
    {
        if (!CanShoot()) return;

        base.Shoot(playerTransform);
        _isShooting = true;
        _zoneCollider.enabled = true;
        _playerGivenDamage = playerTransform.gameObject.GetComponent<Player>();
        InvokeRepeating(nameof(ApplyDamage), 0f, 0.1f);
    }

    public override void ShootFinished()
    {
        base.ShootFinished();
       _isShooting = false;
       _zoneCollider.enabled = false;

        CancelInvoke(nameof(ApplyDamage));
    }

    private void ApplyDamage()
    {
        if (!_isShooting) return;

        foreach (Collider2D enemy in _enemiesInZone)
        {
            if (enemy is not null && enemy.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(stats.damage, _playerGivenDamage);
                Debug.Log($"{enemy.name} take {stats.damage / 10} per seconds and {stats.damage} for all");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerMask) != 0)
        {
            if (!_enemiesInZone.Contains(other))
            {
                _enemiesInZone.Add(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_enemiesInZone.Contains(other))
        {
            _enemiesInZone.Remove(other);
        }
    }
}
