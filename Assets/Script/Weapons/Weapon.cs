using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

struct _stats
{
    public float _fireRange;
    public float _fireRate;
    public float _bulletSpeed;
    public float _damage;
    public Vector3 _aoeRange;
}

public abstract class Weapon : MonoBehaviour
{
    public float _cooldown;
    internal int _playerMask = 6;
    
    internal _stats _stats;
    
    public abstract void Initialize(float _fireRange, float _fireRate, float _damage, Vector3 _aoeRange, float _bulletSpeed, GameObject _bullet);
    public abstract void Shoot(InputAction.CallbackContext _callback);
    public virtual void Reload(){}

    public void Update()
    {
        _cooldown += Time.deltaTime;
    }

    enum WeaponType
    {
        
    }
}
