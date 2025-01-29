using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private bool _isDestroy;
    public float health;

    void Start()
    {
        
    }

    private void TakeDamage(float damage)
    {


        if (health <= 0)
        {
            Destroy(gameObject);
            _isDestroy = true;
        }

        if (_isDestroy)
        {
            // Score = 0
            // Retour au menu principal
            // DropWeapons()
            return;
        }
    }
}
