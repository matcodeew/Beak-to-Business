using UnityEngine;

public class MeleeWeapon : Weapon
{
    private RaycastHit2D[] _allHits;
    public override void Shoot(Transform playerTransform)
    {
        base.Shoot(playerTransform);

        _allHits = Physics2D.BoxCastAll(playerTransform.position + transform.up * transform.localScale.x, stats.aoeRange, 0.0f, transform.up);

        foreach (RaycastHit2D hit in _allHits)
        {
            if (hit.collider.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(stats.damage, playerTransform.GetComponent<Player>());
            }
        }
    }
    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
    }
}
