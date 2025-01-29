using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{
    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet, int _maxBulletAmount)
    {
        _stats = new _stats();
        _stats.fireRange = _fireRange;
        _stats.fireRate = _fireRate;
        _stats.damage = _damage;
        _stats.aoeRange = _aoeRange;
    }

    public override void Shoot(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            if (_cooldown >= _stats.fireRate)
            {
                Vector3 _startPos = transform.position + Vector3.forward * _stats.fireRange / 2;

                RaycastHit[] _hit = Physics.BoxCastAll(_startPos, _stats.aoeRange, Vector3.forward, Quaternion.identity,
                    _stats.fireRange, _playerMask);

                foreach (RaycastHit _raycast in _hit)
                {
                    Debug.Log(_raycast.collider.gameObject.name);
                }
                
                if (_cooldown >= _stats.fireRate * 2)
                    _cooldown = 0;
                else
                    _cooldown -= _stats.fireRate;
            }
        }
    }
}
