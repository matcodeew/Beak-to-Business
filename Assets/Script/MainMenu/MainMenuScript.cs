using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private User _testUser;
    
    public TextMeshProUGUI userIdText;
    public TextMeshProUGUI userNameText;

    public GameObject frontPanel;
    
    private string userName;
    private int userId;
    
    public UnityEvent<int> _onUserLoggedIn;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetPlayerIdCookie();
#endif
        // GetUserId("22");
    }


    [DllImport("__Internal")]
    public static extern void GetPlayerIdCookie();
    public void GetUserId(string _id)
    {
        StartCoroutine(GetUser(_id));
    }
    private IEnumerator GetUser(string _id)
    {
        int _playerId = int.Parse(_id);
        
        var _task = APICaller.GetUserById(_playerId); // This is better as it makes implementation easier, if problem ever rise you can just change it to TestApiCaller() to test
        yield return new WaitUntil(() => _task.IsCompleted);
        _testUser = _task.Result;
        
        userId = _testUser.id;
        userName = _testUser.nickname;
        
        SetPlayerInfos();
        
        _onUserLoggedIn.Invoke(userId);
        
        frontPanel.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    private void SetPlayerInfos()
    {
        userIdText.text = "Id : " + userId.ToString();
        userNameText.text = "Username : <br>" + userName;
    }

    public void PlayGame(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void LogOut()
    {
        
    }
}
