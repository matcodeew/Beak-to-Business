using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{
    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet)
    {
        _stats = new _stats();
        _stats._fireRange = _fireRange;
        _stats._fireRate = _fireRate;
        _stats._damage = _damage;
        _stats._aoeRange = _aoeRange;
    }

    public override void Shoot(InputAction.CallbackContext _context)
    {
        if (_cooldown >= _stats._fireRate)
        {
            Vector3 _startPos = transform.position + Vector3.forward * _stats._fireRange / 2;

            RaycastHit[] _hit = Physics.BoxCastAll(_startPos, _stats._aoeRange, Vector3.forward, Quaternion.identity,
                _stats._fireRange, _playerMask);

            foreach (RaycastHit _raycast in _hit)
            {
                Debug.Log(_raycast.collider.gameObject.name);
            }
            
            if (_cooldown >= _stats._fireRate * 2)
                _cooldown = 0;
            else
                _cooldown -= _stats._fireRate;
        }
    }
}
