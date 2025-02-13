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
    
    [SerializeField] private string _downAnim;
    [SerializeField] private string _upAnim;
    [SerializeField] private string _leftAnim;
    [SerializeField] private string _rightAnim;

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
        //GetComponent<PlayerAudio>().PlaySniperAudio();
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

    public void StartAnim(Transform _playerTransform)
    {
        Vector2 _shootDirection = _playerTransform.GetComponent<PlayerMovement>().heading.normalized;
        
        Debug.Log(_shootDirection);

        if (_shootDirection.x > 0 && _shootDirection.x < 1
            && _shootDirection.y > -0.707f && _shootDirection.y < 0.707f)
        {
            animator.Play(_rightAnim);
            Debug.Log("Right");
        } else if (_shootDirection.x > -1 && _shootDirection.x < 0 
                   && _shootDirection.y > -0.707f && _shootDirection.y < 0.707f)
        {
            animator.Play(_leftAnim);
            Debug.Log("Left");
        } else if (_shootDirection.x > -0.707 && _shootDirection.x < 0.707
                   && _shootDirection.y > 0f && _shootDirection.y < 1f)
        {
            animator.Play(_upAnim);
            Debug.Log("Up");
        } else if (_shootDirection.x > -0.707 && _shootDirection.x < 0.707
                   && _shootDirection.y > -1f && _shootDirection.y < 0f)
        {
            animator.Play(_downAnim);
            Debug.Log("Down");
        }

        // animator.SetFloat("DirectionX", _shootDirection.x);
        // animator.SetFloat("DirectionY", _shootDirection.y);
    }
}
