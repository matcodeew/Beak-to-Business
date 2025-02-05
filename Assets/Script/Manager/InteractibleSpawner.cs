using System.Collections.Generic;
using UnityEngine;

public class InteractibleSpawner : MonoBehaviour
{
    private List<InteractableObjects> _interactibleObjectToSpawn = new();
    [SerializeField] private Vector2 _mapBounds;
    private Vector2 _randPosTemp;
    private void Start()
    {
        foreach (var a in _interactibleObjectToSpawn)
        {
            _randPosTemp = PickRandomPositionOnMap();
            if (CanSpawnHere(_randPosTemp))
            {
                GameObject newObj = Instantiate(a.gameObject, gameObject.transform);
                newObj.transform.position = _randPosTemp;
            }
        }
    }
    private Vector2 PickRandomPositionOnMap() => Random.insideUnitCircle * _mapBounds;

    private bool CanSpawnHere(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, -transform.up);
        if (hit.collider.CompareTag("Floor"))
        {
            return true;
        }
        else if (hit.collider.CompareTag("Wall"))
        {
            return false;
        }
        return false;
    }
}
