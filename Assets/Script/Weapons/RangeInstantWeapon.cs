using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeInstantWeapon : Weapon
{
    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet)
    {
        _stats = new _stats();
        _stats._fireRange = _fireRange;
        _stats._fireRate = _fireRate;
        _stats._damage = _damage;
    }

    public override void Shoot(InputAction.CallbackContext _callback)
    {
        if (_cooldown >= _stats._fireRate)
        {
            Debug.DrawRay(transform.position, transform.forward * _stats._fireRange, Color.green);
            if (Physics.Raycast(transform.position, transform.forward, _stats._fireRange, _playerMask))
            {
                Debug.Log("ennemi touché");
            }
            
            if(_cooldown >= _stats._fireRate * 2)
                _cooldown = 0;
            else
                _cooldown -= _stats._fireRate;
        }
    }
}
