using UnityEngine;


[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float _fireRange;
    public float _fireRate;
    public float _damage;
    public Vector2 _aoeRange;
    public float _bulletSpeed;
}
