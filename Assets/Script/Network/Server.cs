using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Button connectButton;

    public static Server instance;

    [SerializeField] private List<SpawnableObjectOnSpawnOnServer> spawnableObjectsOnSpawn;

    [Header("Item Random Spawn")]
    [SerializeField] private List<GameObject> spawnableRandomItems;
    public Rect spawnZone;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnZone.center, spawnZone.size);
    }


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        connectButton.onClick.AddListener(ConnectToServer);

        string[] args = System.Environment.GetCommandLineArgs();
                //SpawnObjectsOnServer();

        for (int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-s")
            {
                Debug.Log("--------------------Running as server--------------------");
                NetworkManager.Singleton.StartServer();
                NetworkManager.Singleton.OnClientConnectedCallback += ClientConnect;
                //GameObject go = Instantiate(cubePrefab, new Vector3(2.88f, 0.15f, 2.92f), Quaternion.identity);
                //go.GetComponent<NetworkObject>().Spawn();
                SpawnObjectsOnServer();
                RandomSpawnObjectsOnServer();
            }
        }

    }


    private void RandomSpawnObjectsOnServer()
    {
        foreach (GameObject obj in spawnableRandomItems)
        {
            Vector2 position = new Vector2(Random.Range(spawnZone.xMin, spawnZone.xMax), Random.Range(spawnZone.yMin, spawnZone.yMax));
            GameObject gameObj = Instantiate(obj, position, Quaternion.identity);
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

    [Rpc(SendTo.Server)]
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
        //ScoreboardManager.instance.OnPlayerEnteredRoom(connectionID);

    }


        void ConnectToServer()
        {
        connectButton.gameObject.SetActive(false);
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
