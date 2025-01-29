using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeInstantWeapon : Weapon
{
    public override void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet, int _maxBulletAmount)
    {
        _stats = new _stats();
        _stats.fireRange = _fireRange;
        _stats.fireRate = _fireRate;
        _stats.damage = _damage;
        
        _stats.maxBulletAmount = currentBulletAmount = _maxBulletAmount;
    }

    public override void Shoot(InputAction.CallbackContext _callback)
    {
        if (_callback.phase == InputActionPhase.Performed)
        {
            if (_cooldown >= _stats.fireRate && currentBulletAmount > 0)
            {
                Debug.DrawRay(transform.position, transform.forward * _stats.fireRange, Color.green);
                if (Physics.Raycast(transform.position, transform.forward, _stats.fireRange, _playerMask))
                {
                    Debug.Log("ennemi touché");
                }

                currentBulletAmount -= 1;
                
                if(_cooldown >= _stats.fireRate * 2)
                    _cooldown = 0;
                else
                    _cooldown -= _stats.fireRate;
            }
        }
    }
}
