using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Server : NetworkBehaviour
{

    [SerializeField] private string serverAddress = "192.168.1.238";
    [SerializeField] private ushort serverPort = 7777;

    [SerializeField] private Button connectButton;

    [SerializeField] private List<NetworkObject> _objectsToSpawn;

    private NetworkList<NetworkObjectReference> _destroyedObjects = new NetworkList<NetworkObjectReference>();

    public static Server instance;

    //test
    public GameObject cubePrefab;


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
            }
        }

    }

    private void SpawnObjectsOnServer()
    {
        foreach (var obj in _objectsToSpawn)
        {
            //obj.Spawn();
            if (obj.IsSpawned) obj.gameObject.SetActive(true);
        }

    }

    //[ServerRpc(RequireOwnership = false)]
    [Rpc(SendTo.Server)]
    public void DestroyObjectServerRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj))
        {
            obj.Despawn();
            //Destroy(obj.gameObject);
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
