using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPICall : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private string url = "http://192.168.1.238/api/api.php?endpoint=users";
    [SerializeField] private string userid = "22";

    void Start()
    {
        StartCoroutine(GetUserFromId());
    }
    
    IEnumerator GetUsers(){
        UnityWebRequest request = UnityWebRequest.Get(url);
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Erreur ça marche pas au secours" + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            
            Debug.Log("y'a réponse" + jsonResponse);
            
            User[] users = JsonHelper.FromJson<User>(jsonResponse);
            
            foreach (User user in users)
            {
                Debug.Log("ID:" + user.id + ", Name:" + user.nickname + ", Email:" + user.email + ", Password:" + user.password);
            }
            
        }
    }

    IEnumerator GetUserFromId()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/" + userid);
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("YA PAS YA PAS" + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            
            Debug.Log("TOUCHE TOUCHE" + jsonResponse);

            User user = JsonHelper.FromSingleJson<User>(jsonResponse);
            
            debugText.text = "ID:" + user.id + ",<br> Name:" + user.nickname + ",<br> Email:" + user.email + ",<br> Password:" + user.password;
            Debug.Log("ID:" + user.id + ", Name:" + user.nickname + ", Email:" + user.email + ", Password:" + user.password);
        }
    }
}



[System.Serializable]
public class User
{
    public int id;
    public string nickname;
    public string email;
    public string password;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }
  
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
    
    public static T FromSingleJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}

