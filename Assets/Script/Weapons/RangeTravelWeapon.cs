using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    private GameObject _currentBullet = null;
    private GameObject _bulletPrefab;
    private Vector2 _endPos;
    private Vector2 _startPos;
    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
        _bulletPrefab = data.bulletPrefab;
    }
    public override void Shoot(Transform playerTransform)
    {
        ResetData();
        base.Shoot(playerTransform);
        _startPos = (playerTransform.position + playerTransform.up * playerTransform.localScale.x);
        _endPos = _startPos + (Vector2)playerTransform.up * stats.fireRange;

        if (_currentBullet == null)
        {
            _currentBullet = Instantiate(_bulletPrefab, _startPos, Quaternion.identity); //do instead GetObjectFromPool(type Bulet);
            _currentBullet.GetComponent<Bullet>().InitializeBullet(_startPos, _endPos, stats.bulletSpeed, playerTransform.gameObject.GetComponent<Player>());
        }
        else if (_currentBullet != null)
        {
            _currentBullet.transform.position = playerTransform.position;
        }
    }
    private void ResetData()
    {
        _currentBullet = null;
        _endPos = Vector2.zero;
        _startPos = Vector2.zero;
    }
}
