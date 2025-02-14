using UnityEngine;

public class MeleeWeapon : Weapon
{
    private RaycastHit2D[] _allHits;
    public override bool Shoot(Transform playerTransform)
    {
       if(!base.Shoot(playerTransform)) return false;

        _allHits = Physics2D.BoxCastAll(playerTransform.position + transform.up * transform.localScale.x, stats.aoeRange, 0.0f, transform.up);

        foreach (RaycastHit2D hit in _allHits)
        {
            if (hit.collider.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(stats.damage);
            }
        }
        return true;
    }

    public override GameObject GetBulletPrefab()
    {
        return null;
    }

    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
    }
}
