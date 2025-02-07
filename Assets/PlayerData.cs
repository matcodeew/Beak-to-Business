using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<int> Position = new NetworkVariable<int>();
    
    public NetworkVariable<FixedString128Bytes> Name = new NetworkVariable<FixedString128Bytes>();

    [SerializeField] private TextMeshProUGUI _personalScore;

    

    public override void OnNetworkSpawn()
    {
        
        Score.OnValueChanged += UpdateScore;

        base.OnNetworkSpawn();
        if (!IsServer) return;

        Score.Value = 0;
        GetNameClientRPC(
            new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { GetComponent<NetworkObject>().OwnerClientId }
                }
            });
    }

    private void Update()
    {
        if(IsOwner)
        {
            if(Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Increase Score");
                IncreaseScoreServerRpc(10);

            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreaseScoreServerRpc(int value)
    {
        Score.Value += value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetScoreServerRpc(int value)
    {
        Score.Value = value;
    }



    [ClientRpc]
    public void GetNameClientRPC(ClientRpcParams clientRpcParams = default)
    {
        //Mettre le nom récupéré dans la BDD a la place de "Player"
        GetNameServerRPC("Player");
    }

    [ServerRpc]
    public void GetNameServerRPC(string name)
    {
        this.Name.Value = name;
    }

    private void UpdateScore(int previousValue, int newValue)
    {
        _personalScore.text = "score : " + newValue.ToString();
    }
}
