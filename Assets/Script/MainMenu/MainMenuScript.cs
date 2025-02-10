using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{
    private User _testUser;
    
    public TextMeshProUGUI userIdText;
    public TextMeshProUGUI userNameText;

    public GameObject frontPanel;
    
    private string userName;
    private int userId;
    
    public UnityEvent<int> _onUserLoggedIn;

    
    void Start()
    {
        StartCoroutine(GetUser(22));
    }


    private IEnumerator GetUser(int _id)
    {
        var
            _task = APICaller
                .GetUserById(
                    _id); // This is better as it makes implementation easier, if problem ever rise you can just change it to TestApiCaller() to test
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
}
