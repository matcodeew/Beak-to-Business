using Unity.Netcode;
using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    private GameObject _bulletPrefab;
    private Vector2 _endPos;
    private Vector2 _startPos;

    private Vector3 direction = Vector2.up;

    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
        _bulletPrefab = data.bulletPrefab;
    }

    public override void Shoot(Transform playerTransform)
    {
        ResetData();
        base.Shoot(playerTransform);

        direction = playerTransform.GetComponent<PlayerMovement>().direction;
        _startPos = playerTransform.position + direction * playerTransform.localScale.x;

        playerTransform.GetComponent<Player>().RequestSpawnBullet(_startPos, direction);
    }

    public override GameObject GetBulletPrefab()
    {
        return _bulletPrefab;
    }

    private void ResetData()
    {
        _endPos = Vector2.zero;
        _startPos = Vector2.zero;
    }
}
