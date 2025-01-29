using UnityEngine;


[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float fireRange;
    public float fireRate;
    public float damage;
    public Vector2 aoeRange;
    public float bulletSpeed;

    public int maxBulletAmount;
}
