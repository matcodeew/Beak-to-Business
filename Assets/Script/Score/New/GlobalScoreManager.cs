using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GlobalScoreManager : MonoBehaviour
{
    [SerializeField] private Transform scoreBoard;
    [SerializeField] private GameObject playerScoreTemplate;
    List<GameObject> playerTemplates = new List<GameObject>();

    public static GlobalScoreManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerNetwork.OnPlayerSpawn += OnPlayerSpawned;
    }


    private void OnPlayerSpawned(GameObject player, ulong connectionID)
    {
        GameObject playerUI = Instantiate(playerScoreTemplate, scoreBoard);
        playerUI.GetComponent<PlayerScore>().TrackPlayer(player, connectionID);
        playerTemplates.Add(playerUI);
        Debug.Log("test");
        Debug.Log(NetworkManager.Singleton.ConnectedClients);
        Invoke("UpdateOrder", .5f);
    }

    

    public void UpdateOrder() //call this with event when one of player Update Score
    {
        playerTemplates = playerTemplates.OrderByDescending(obj => ConvertScore(obj)).ToList();

        int a = playerTemplates.Count > 10 ? 10 : playerTemplates.Count;

        for (int i = 0; i < a; i++)
        {
            if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(playerTemplates[i].GetComponent<PlayerScore>().connectionID))
            {
                playerTemplates[i].SetActive(false);
                playerTemplates.RemoveAt(i);
                i--;
                a--;
                continue;
            }
            if (NetworkManager.Singleton.ConnectedClients[playerTemplates[i].GetComponent<PlayerScore>()
                    .connectionID].PlayerObject.GetComponent<PlayerDeath>()._isDead.Value)
            {
                playerTemplates[i].SetActive(false);
            }
            else
            {
                playerTemplates[i].SetActive(true);
            }

            playerTemplates[i].transform.SetSiblingIndex(i);
            playerTemplates[i].GetComponent<PlayerScore>().ChangePosition(i + 1);
        }
    }

    int ConvertScore(GameObject go)
    {
        PlayerScore playerScore = go.GetComponent<PlayerScore>();
        if (playerScore != null)
        {
            int score;
            if (int.TryParse(playerScore.scoreUI.text, out score))
            {
                return score;
            }
        }
        return 0;
    }

}
