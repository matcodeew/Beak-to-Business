using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    public float speed;
    public float health;
    public int XP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject nearestObject = collider.gameObject;
        if (nearestObject is not null)
        {
            InteractableObjects interactible = nearestObject.GetComponent<InteractableObjects>();
            if (interactible is not null)
            {
                interactible.PlayerInteract();
                Destroy(nearestObject);
                return;
            }
        }
    }
}