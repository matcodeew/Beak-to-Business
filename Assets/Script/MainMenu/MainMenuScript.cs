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
        //Comment� = build Web
        //Non comment� = nuild Windows
        GetUserId("22");
    }

    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        //SpawnObjectsOnServer();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-s")
            {
                Debug.Log("--------------------Running as server--------------------");
                //NetworkManager.Singleton.StartServer();
                SceneManager.LoadScene("GameRoom");
                
            }
        }
    }

    [DllImport("__Internal")]
    public static extern void GetPlayerIdCookie();
    
    [DllImport("__Internal")]
    public static extern void LogOut();
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
        SavePlayerInfos(_testUser);
        
        _onUserLoggedIn.Invoke(userId);
        
        frontPanel.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    private void SetPlayerInfos()
    {
        userIdText.text = "Id : " + userId.ToString();
        userNameText.text = "Username : <br>" + userName;
    }

    private void SavePlayerInfos(User user)
    {
        GameObject userInfo = new GameObject("UserInfo");
        userInfo.AddComponent<UserInfos>();
        userInfo.GetComponent<UserInfos>().id = user.id;
        userInfo.GetComponent<UserInfos>().nickname = user.nickname;
        DontDestroyOnLoad(userInfo);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void logOut()
    {
        LogOut();
    }
}
