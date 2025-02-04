using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
    private List<GameObject> _enemiesInZone = new List<GameObject>();
    private bool _isShooting = false;
    private Player _playerGivenDamage;
    [SerializeField] float coolDownTimer;
    private Vector2 _startPos;

    public override void Shoot(Transform playerTransform)
    {
        base.Shoot(playerTransform);
        _isShooting = true;
        _playerGivenDamage = playerTransform.gameObject.GetComponent<Player>();
        InvokeRepeating(nameof(ApplyDamage), 0f, 0.1f);
    }

    public override void ShootFinished()
    {
        base.ShootFinished();
        _isShooting = false;
        CancelInvoke(nameof(ApplyDamage));
    }

    public override GameObject GetBulletPrefab()
    {
        return null;
    }

    private void ApplyDamage()
    {
        if (!_isShooting) return;
        _enemiesInZone.Clear();

        _startPos = _playerGivenDamage.transform.position + _playerGivenDamage.transform.up *
            ((_playerGivenDamage.transform.localScale.x / 2) + stats.aoeRange.y / 2);


        RaycastHit2D[] _allHit = Physics2D.BoxCastAll(_startPos, stats.aoeRange, 0.0f, _playerGivenDamage.transform.up, 0.0f, playerMask);

        foreach (var hit in _allHit)
        {
            _enemiesInZone.Add(hit.collider.gameObject);
        }
        foreach (GameObject enemy in _enemiesInZone)
        {
            if (enemy is not null && enemy.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(stats.damage / 10, _playerGivenDamage);
                Debug.Log($"{enemy.name} take {stats.damage / 10}  and {stats.damage} per second");
            }
        }
    }
}
