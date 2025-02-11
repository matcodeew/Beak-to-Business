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

    private PlayerData _playerData;
    public void TrackPlayer(GameObject player, ulong connectionID)
    {
        _playerData = player.GetComponent<PlayerData>();
        if (_playerData is null) return;
        
        _playerData.Name.OnValueChanged += OnNameChanged;
        _playerData.Score.OnValueChanged += OnScoreChanged;
        OnScoreChanged(0, _playerData.Score.Value);
        OnNameChanged("", _playerData.Name.Value);
        this.connectionID = connectionID;
    }

    public void ChangePosition(int newValue)
    {
        positionUI.text = newValue.ToString() + ".";
    }

    private void OnScoreChanged(int previousValue, int newValue)
    {
        scoreUI.text = newValue.ToString();
        GlobalScoreManager.instance.UpdateOrder();
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        nameUI.text = newValue.ToString();
    }
}
