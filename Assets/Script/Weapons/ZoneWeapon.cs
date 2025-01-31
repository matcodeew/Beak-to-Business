using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoneWeapon : RangeTravelWeapon
{
    public Collider zoneCollider;

    public override void Shoot(InputAction.CallbackContext _callback)
    {
        if (_callback.phase == InputActionPhase.Started)
        {
            zoneCollider.enabled = true;
        }

        if (_callback.phase == InputActionPhase.Canceled)
        {
            zoneCollider.enabled = false;
        }
        
        Debug.Log(zoneCollider.enabled);
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_cooldown >= _stats.fireRate && currentBulletAmount > 0)
        {
            if (_other.gameObject.layer == _playerMask)
            {
                Debug.Log("hit");
            }
            
            currentBulletAmount -= 1;

            if (_cooldown >= _stats.fireRate * 2)
            {
                _cooldown = 0;
            } else {
                _cooldown -= _stats.fireRate;
            }
        }
    }
}
