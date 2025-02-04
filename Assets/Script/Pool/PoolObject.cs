using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ObjectType
{
    NULL,
    Bullet,
}
public class PoolObject : MonoBehaviour
{
    [Header("TEST")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int BulletPoolSize;
    [SerializeField] private Transform parent;
    private GameObject BulletSpawned;

    public Dictionary<ObjectType, List<GameObject>> poolObject = new();

    private void Start()
    {
        InitializePool(ObjectType.Bullet, BulletPrefab, BulletPoolSize, parent);
    }

    #region TEST Function
    [ContextMenu("SpawnBullet")]
    public void SpawnBullet()
    {
        GameObject newBullet = GetObjectFromPool(ObjectType.Bullet);
        newBullet.transform.position = parent.position;
        BulletSpawned = newBullet;  
    }

    [ContextMenu("ReturnBulletToPool")]
    public void ReturnBulletToPool()
    {
        ReturnToPool(BulletSpawned, ObjectType.Bullet);
    }
    #endregion
    private void InitializePool(ObjectType poolType, GameObject ObjectPrefab, int poolSize, Transform parent)
    {
        if (poolType == ObjectType.NULL) throw new ArgumentNullException(nameof(poolType), "Pool Type cannot be NULL.");
        if (ObjectPrefab is null) throw new ArgumentNullException(nameof(ObjectPrefab), "ObjectPrefab cannot be null.");
        if (poolSize <= 0) throw new ArgumentOutOfRangeException(nameof(poolSize), "Pool size must be positive.");
        if (parent is null) throw new ArgumentNullException(nameof(poolType), "parent cannot be null.");

        if (poolObject.ContainsKey(poolType))
        {
            return;
        }

        poolObject.Add(poolType, new List<GameObject>());
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObj = Instantiate(ObjectPrefab, parent);
            poolObject[poolType].Add(newObj);
            newObj.SetActive(false);
        }
    }
    public GameObject GetObjectFromPool(ObjectType type)
    {
        if (!poolObject.ContainsKey(type))
        {
            return null;
        }

        foreach (GameObject enemy in poolObject[type])
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject objects, ObjectType type)
    {
        if (!poolObject.ContainsKey(type)) { return; }

        objects.SetActive(false);
        objects.transform.position = Vector3.zero;

    }
}
