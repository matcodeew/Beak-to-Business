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

    private void Awake()
    {
        instance = this;
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
