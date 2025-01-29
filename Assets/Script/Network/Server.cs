using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Server : NetworkBehaviour
{

    [SerializeField] private string serverAddress = "192.168.1.238";
    [SerializeField] private ushort serverPort = 7777;

    [SerializeField] private Button connectButton;

    void Start()
    {
        connectButton.onClick.AddListener(ConnectToServer);


        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-s")
            {
                Debug.Log("--------------------Running as server--------------------");
                NetworkManager.Singleton.StartServer();
               
                NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectMessage;
                StartCoroutine(PingClients());
            }
        }
    }

    public void ClientConnectMessage(ulong connectionID)
    {
        Debug.Log("----------" + connectionID + " connected----------");
    }

    public IEnumerator PingClients()
    {
        yield return new WaitForSeconds(5f);

        while(NetworkManager.Singleton.IsServer)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("Sending pings...");
            PingClientRpc();
        }
    }

    [ClientRpc]
    public void PingClientRpc()
    {
        Debug.Log("Got Ping From Server");
    }


    void ConnectToServer()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager non trouvé !");
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        if (transport != null)
        {
            transport.ConnectionData.Address = serverAddress;
            transport.ConnectionData.Port = serverPort;

            NetworkManager.Singleton.StartClient();
            Debug.Log("Tentative de connexion au serveur...");
        }
        else
        {
            Debug.LogError("Transport Unity non trouvé !");
        }
    }

}
