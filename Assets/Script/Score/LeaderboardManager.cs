using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] List<GameObject> list;

    public Transform namesContainer;
    public Transform scoresContainer;
    public Transform rankingContainer;

    public GameObject textPrefab;

    void Start()
    {
        // Exemple de données (tu peux remplacer cela par des scores récupérés d'une base de données)
        AddScore("Alice", 150);
        AddScore("Bob", 200);
        AddScore("Charlie", 100);

        //UpdateLeaderboard();

        list[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "sdtfd";
    }

    public void AddScore(string name, int score)
    {
        //playerScores.Add(new PlayerScore { playerName = name, score = score });
        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        // Trier par score décroissant
        //playerScores.Sort((a, b) => b.score.CompareTo(a.score));

        // Nettoyer l'affichage précédent
        //foreach (Transform child in namesContainer) Destroy(child.gameObject);
        //foreach (Transform child in scoresContainer) Destroy(child.gameObject);
        //foreach (Transform child in rankingContainer) Destroy(child.gameObject);

        // Remplir le leaderboard
        //for (int i = 0; i < playerScores.Count; i++)
        //{
        //    CreateText(namesContainer, playerScores[i].playerName);
        //    CreateText(scoresContainer, playerScores[i].score.ToString());
        //    CreateText(rankingContainer, (i + 1).ToString());
        }
    }

