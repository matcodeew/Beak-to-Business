using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPICall : MonoBehaviour
{
    //This class contains various example of how to call the API
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private string url = "http://192.168.1.238/api/api.php?endpoint=users";
    [SerializeField] private int userid = 22;
    private User _testUser;

    void Start()
    {
        StartCoroutine(InitUser(userid));
    }

    private IEnumerator InitUser(int id)
    {
        var
            task = APICaller
                .GetUserById(
                    id); // This is better as it makes implementation easier, if problem ever rise you can just change it to TestApiCaller() to test
        yield return new WaitUntil(() => task.IsCompleted);
        _testUser = task.Result;
        debugText.text = "ID:" + _testUser.id + ", Name:" + _testUser.nickname + ", Email:" + _testUser.email + ", Password:" +
                  _testUser.password;
    }

    private async Task<User> TestApiCaller()
    {
        return await APICaller.GetUserById(22);
    }
}

//IEnumerator GetUsers(){
    //    UnityWebRequest request = UnityWebRequest.Get(url);
    //    
    //    yield return request.SendWebRequest();
//
    //    if (request.result == UnityWebRequest.Result.ConnectionError ||
    //        request.result == UnityWebRequest.Result.ProtocolError)
    //    {
    //        Debug.Log("Erreur ça marche pas au secours" + request.error);
    //    }
    //    else
    //    {
    //        string jsonResponse = request.downloadHandler.text;
    //        
    //        Debug.Log("y'a réponse" + jsonResponse);
    //        
    //        User[] users = JsonHelper.FromJson<User>(jsonResponse);
    //        
    //        foreach (User user in users)
    //        {
    //            Debug.Log("ID:" + user.id + ", Name:" + user.nickname + ", Email:" + user.email + ", Password:" + user.password);
    //        }
    //        
    //    }
    //}
    //IEnumerator GetUserFromId()
    //{
    //    UnityWebRequest request = UnityWebRequest.Get(url + "/" + userid);
    //    
    //    yield return request.SendWebRequest();
//
    //    if (request.result == UnityWebRequest.Result.ConnectionError ||
    //        request.result == UnityWebRequest.Result.ProtocolError)
    //    {
    //        Debug.Log("YA PAS YA PAS" + request.error);
    //    }
    //    else
    //    {
    //        string jsonResponse = request.downloadHandler.text;
    //        
    //        Debug.Log("TOUCHE TOUCHE" + jsonResponse);
//
    //        User user = JsonHelper.FromSingleJson<User>(jsonResponse);
    //        
    //        debugText.text = "ID:" + user.id + ",<br> Name:" + user.nickname + ",<br> Email:" + user.email + ",<br> Password:" + user.password;
    //        Debug.Log("ID:" + user.id + ", Name:" + user.nickname + ", Email:" + user.email + ", Password:" + user.password);
    //    }
    //}
//}



