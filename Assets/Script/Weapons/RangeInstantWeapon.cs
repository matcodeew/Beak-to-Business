using UnityEngine;

public class RangeInstantWeapon : Weapon
{
    private RaycastHit2D _hit;
    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
    }
    public override void Shoot(Transform playerTransform)
    {
        base.Shoot(playerTransform);
        _hit = Physics2D.Raycast(playerTransform.position, playerTransform.up, stats.fireRange, playerMask);
        if (_hit)
        {
            print($"{_hit.collider.name} hitted");
            if (_hit.collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(stats.damage, playerTransform.gameObject.GetComponent<Player>());
            }
        }
    }
}
