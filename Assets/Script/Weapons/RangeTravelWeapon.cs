using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    internal GameObject bulletPrefab;
    private GameObject _currentBullet;

    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet, int _maxBulletAmount)
    {
        _stats = new _stats();
        _stats.fireRange = _fireRange;
        _stats.fireRate = _fireRate;
        _stats.damage = _damage;
        _stats.bulletSpeed = _bulletSpeed;
        bulletPrefab = _bullet;

        _stats.maxBulletAmount = currentBulletAmount = _maxBulletAmount;
    }

    public override void Shoot(Transform p)
    {
        if (_cooldown >= _stats.fireRate && currentBulletAmount > 0)
        {
            if (_currentBullet == null)
            {
                _currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                _currentBullet.transform.position = transform.position;
            }

            currentBulletAmount -= 1;

            _currentBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            _currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);


            if (_cooldown >= _stats.fireRate * 2)
                _cooldown = 0;
            else
                _cooldown -= _stats.fireRate;
        }
    }
}
