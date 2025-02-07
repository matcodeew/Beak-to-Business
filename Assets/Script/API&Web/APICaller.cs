using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class APICaller
{
    //We make the class static so it can be accessed everywhere at any times
    //It is also asynchronous since static classes do not support Coroutines
    private static string api_url = "http://192.168.1.238/api/api.php?endpoint=";

    public static async Task<User> GetUserById(int id)
    {
        return await GetUserByIdRequest(id);
    }

    //This is just like the coroutine example in the TestAPICall script, except made to be used in a static class
    private static async Task<User> GetUserByIdRequest(int id)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(api_url + "users/" + id))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                return null;
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                return JsonHelper.FromSingleJson<User>(jsonResponse);
            }
        }
    }
}

//These classes are used to format the Json from the api into a User object
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