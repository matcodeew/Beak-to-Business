using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI positionUI;
    public ulong connectionID;

    public void TrackPlayer(GameObject player, ulong connectionID)
    {
        player.GetComponent<PlayerData>().Name.OnValueChanged += OnNameChanged;
        player.GetComponent<PlayerData>().Score.OnValueChanged += OnScoreChanged;
        OnScoreChanged(0, player.GetComponent<PlayerData>().Score.Value);
        OnNameChanged("", player.GetComponent<PlayerData>().Name.Value);
        this.connectionID = connectionID;
    }

    public void ChangePosition(int newValue)
    {
        positionUI.text = newValue.ToString() + ".";
    }

    private void OnScoreChanged(int previousValue, int newValue)
    {
        scoreUI.text = newValue.ToString();
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        nameUI.text = newValue.ToString();
    }
}
