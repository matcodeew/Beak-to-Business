using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    private GameObject _bulletPrefab;
    private Vector2 _startPos;

    private Vector3 direction = Vector2.up;

    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
        _bulletPrefab = data.bulletPrefab;
    }

    public override bool Shoot(Transform playerTransform)
    {
        //ResetData();

        if (!base.Shoot(playerTransform)) { return false; }

        direction = playerTransform.GetComponent<PlayerMovement>().direction;
        _startPos = playerTransform.position + direction * playerTransform.localScale.x;

        playerTransform.GetComponent<Player>().RequestSpawnBullet(_startPos, direction);
        return true;
    }

    public override GameObject GetBulletPrefab()
    {
        return _bulletPrefab;
    }

    private void ResetData()
    {
        _startPos = Vector2.zero;
    }
}
