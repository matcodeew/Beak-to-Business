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
            }
        }
    }

    public void ClientConnectMessage(ulong connectionID)
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
