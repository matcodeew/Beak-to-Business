using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GlobalScoreManager : MonoBehaviour
{
    [SerializeField] private Transform scoreBoard;
    [SerializeField] private GameObject playerScoreTemplate;
    List<GameObject> playerTemplates = new List<GameObject>();

    private void OnEnable()
    {
        PlayerNetwork.OnPlayerSpawn += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        GameObject playerUI = Instantiate(playerScoreTemplate, scoreBoard);
        playerUI.GetComponent<PlayerScore>().TrackPlayer(player);
        playerTemplates.Add(playerUI);
    }

    private void Update()
    {
        UpdateOrder();
    }

    public void UpdateOrder()
    {
        playerTemplates = playerTemplates.OrderByDescending(obj => ConvertScore(obj)).ToList();
        for (int i = 0; i < playerTemplates.Count; i++)
        {
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
