using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SpawnableObjectOnSpawnOnServer
{
    public GameObject spawnedObject;
    public Vector2 spawnPosition;
}

public class Server : NetworkBehaviour
{

    [SerializeField] private string serverAddress = "192.168.1.238";
    [SerializeField] private ushort serverPort = 7777;

    public static Server instance;

    [SerializeField] private List<SpawnableObjectOnSpawnOnServer> spawnableObjectsOnSpawn;

    [Header("Item Random Spawn")]
    public List<Transform> spawnPoints;
    [HideInInspector] public List<Transform> usedSpawnPoints;
    [SerializeField] private List<GameObject> spawnableRandomItems;
    //public Rect spawnZone;

    private bool server = false;

    public Rect spawnZone;

    [Header("Buff")]
    [SerializeField] private int healBuffCountOnMap = 10;
    [SerializeField] private GameObject _healPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spawnZone.center, spawnZone.size);
    }

    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-s")
            {
                server = true;
                Debug.Log($"--------------- Scene {SceneManager.GetActiveScene().name} loaded ---------------");
                NetworkManager.Singleton.StartServer();
                NetworkManager.Singleton.OnClientConnectedCallback += ClientConnect;
                SpawnObjectsOnServer();
                RandomSpawnObjectsOnServer();
                SpawnBuffsOnMap();
            }
        }

        if (!server)
            ConnectToServer();
    }

    private void RandomSpawnObjectsOnServer()
    {
        foreach (GameObject obj in spawnableRandomItems)
        {
            SpawnObj(obj);
            
        }
    }

    private void SpawnBuffsOnMap()
    {
        for(int i =0;i < healBuffCountOnMap; i++)
        {
            GameObject buff = Instantiate(_healPrefab, GetRandomPointOnMap(), Quaternion.identity);
            buff.GetComponent<NetworkObject>().Spawn();
        }
    }

    private Vector2 GetRandomPointOnMap()
    {
        Vector2 position = new Vector2(UnityEngine.Random.Range(spawnZone.min.x, spawnZone.max.x), UnityEngine.Random.Range(spawnZone.min.y, spawnZone.max.y));
        Collider2D[] col = Physics2D.OverlapCircleAll(position, 0.25f);
        if(col.Length == 0)
        {
            return position;
        }
        else
        {
            return GetRandomPointOnMap();

        }
    }

    private void SpawnObj(GameObject obj)
    {
        Transform spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        if(usedSpawnPoints.Contains(spawn))
        {
            SpawnObj(obj);
        }
        else
        {
            usedSpawnPoints.Add(spawn);
            GameObject gameObj = Instantiate(obj, spawn.position, Quaternion.identity);
            gameObj.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void SpawnObjectsOnServer()
    {
        foreach (SpawnableObjectOnSpawnOnServer obj in spawnableObjectsOnSpawn)
        {
            GameObject gameObj = Instantiate(obj.spawnedObject, obj.spawnPosition, Quaternion.identity);
            gameObj.GetComponent<NetworkObject>().Spawn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectOnServerRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj))
        {
            obj.Despawn();
        }
    }

    public void ClientConnect(ulong connectionID)
    {
        Debug.Log("----------" + connectionID + " connected----------");

    }


        void ConnectToServer()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager not found !");
                return;
            }

            var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            if (transport != null)
            {
                transport.ConnectionData.Address = serverAddress;
                transport.ConnectionData.Port = serverPort;

                NetworkManager.Singleton.StartClient();
                Debug.Log("Connecting to server...");
            }
            else
            {
                Debug.LogError("Unity Transport not found !");
            }
        }

}
