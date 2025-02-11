using UnityEngine;

public class TESTInteractWeapon : MonoBehaviour
{
    public Player player;
    public GameObject weapon;
    [ContextMenu("SetPlayerWeapon")]
    public void SetPlayerWeapon()
    {
        player.SetWeapon(weapon.GetComponent<Weapon>(), weapon.GetComponent<Weapon>().weaponData);
        GameObject newWeapon = Instantiate(weapon);
        newWeapon.transform.parent = player.transform;
    }
}
