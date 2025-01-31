using UnityEngine;


[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float fireRange;
    public float fireRate;
    public float damage;
    public Vector3 aoeRange;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public WeaponType type;
}
