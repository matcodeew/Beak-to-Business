using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

struct _stats
{
    public float fireRange;
    public float fireRate;
    public float bulletSpeed;
    public float damage;
    public Vector3 aoeRange;

    public int maxBulletAmount;
}

public abstract class Weapon : MonoBehaviour
{
    public float _cooldown;
    internal int _playerMask = 6;
    
    internal _stats _stats;
    public WeaponStats WeaponStats;

    public int currentBulletAmount;
    
    public abstract void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet, int _maxBulletAmount);
    public abstract void Shoot(Transform playertransform);
    public virtual void ShootFinished() { }

    public virtual void Reload(InputAction.CallbackContext _callback)
    {
        if (_callback.started && currentBulletAmount < _stats.maxBulletAmount)
        {
            currentBulletAmount = _stats.maxBulletAmount;
            Debug.Log(currentBulletAmount);
        }
    }

    public void Update()
    {
        _cooldown += Time.deltaTime;
    }

    enum WeaponType
    {
        
    }
}
