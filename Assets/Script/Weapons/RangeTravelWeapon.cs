using UnityEngine;
using UnityEngine.InputSystem;

public class RangeTravelWeapon : Weapon
{
    private GameObject bulletPrefab;
    private GameObject _currentBullet;
    
    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet)
    {
        _stats = new _stats();
        _stats._fireRange = _fireRange;
        _stats._fireRate = _fireRate;
        _stats._damage = _damage;
        _stats._bulletSpeed = _bulletSpeed;
        bulletPrefab = _bullet;
    }

    public override void Shoot(InputAction.CallbackContext _callback)
    {
        if (_cooldown >= _stats._fireRate)
        {
            if (_currentBullet == null)
            {
                _currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                _currentBullet.transform.position = transform.position;
            }
            
            _currentBullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            _currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
            
            
            if (_cooldown >= _stats._fireRate * 2)
                _cooldown = 0;
            else
                _cooldown -= _stats._fireRate;
        }
    }
}
