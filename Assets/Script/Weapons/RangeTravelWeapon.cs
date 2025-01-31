using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    private GameObject _currentBullet;
    private Rigidbody2D _bulletRigidbody;
    private GameObject _bulletPrefab;
    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
        _bulletPrefab = data.bulletPrefab;
    }
    public override void Shoot(Transform playerTransform)
    {
        base.Shoot(playerTransform);
        if (_currentBullet is null)
        {
            _currentBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity); //do instead GetObjectFromPool(type Bulet);
            _bulletRigidbody = _currentBullet.GetComponent<Rigidbody2D>();
        }
        else
        {
            _currentBullet.transform.position = transform.position;
        }
        _bulletRigidbody.linearVelocity = Vector3.zero;
        _bulletRigidbody.AddForce(transform.forward * 20f);
    }
}
