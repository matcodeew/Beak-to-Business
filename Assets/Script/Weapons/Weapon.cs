using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public struct WeaponsStats
{
    public float fireRange;
    public float fireRate;
    public float bulletSpeed;
    public float damage;
    public Vector2 aoeRange;
}
public enum WeaponType
{

}

public abstract class Weapon : NetworkBehaviour
{
    public WeaponsStats stats;
    protected WeaponType type;
    protected float cooldown;
    [SerializeField] protected LayerMask playerMask;
    public WeaponStats weaponData;

    public GameObject spawnableObject;

    public Animator animator;

    public Sprite weaponImage;

    public virtual void Initialize(WeaponStats data)
    {
        weaponData = data;
        stats.aoeRange = data.aoeRange;
        stats.damage = data.damage;
        stats.fireRange = data.fireRange;
        stats.fireRate = data.fireRate;
        stats.bulletSpeed = data.bulletSpeed;
        
        cooldown = stats.fireRate;
    }

    public abstract GameObject GetBulletPrefab();

    public virtual void Shoot(Transform playerTransform)
    {
        if (!CanShoot()) { return; }
        cooldown = stats.fireRate;
        StartAnim(playerTransform);
    }
    public virtual void ShootFinished() { }

    public virtual void ShootHandler(float delta)
    {
        if (cooldown > 0)
        {
            cooldown -= delta; 
        }
    }
    protected bool CanShoot() => cooldown <= 0;

    public virtual void StartAnim(Transform _playerTransform)
    {
        animator.SetFloat("DirectionX", _playerTransform.GetComponent<PlayerMovement>().moveInput.x);
        animator.SetFloat("DirectionY", _playerTransform.GetComponent<PlayerMovement>().moveInput.y);
    }
}
