using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class APICaller
{
    /// <summary>
    /// This class is used to call the API from anywhere in the game
    ///
    /// Usage is simple, call in an IEnumerator one of the function to get to the desired endpoint of the API :
    ///
    /// private IEnumerator InitUser(int id)
    ///{
    ///    var task = APICaller.GetUserById(id);
    ///    yield return new WaitUntil(() => task.IsCompleted);
    ///    [User variable] = task.Result;
    ///}
    ///
    /// Simply replace the task function with what you'd like to call.
    /// </summary>
    
    
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
    
    public static async Task<Skin> GetSkinById(int id)
    {
        return await GetSkinByIdRequest(id);
    }

    private static async Task<Skin> GetSkinByIdRequest(int id)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(api_url + "skins/" + id))
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
                return JsonHelper.FromSingleJson<Skin>(jsonResponse);
            }
        }
    }

    public static async Task UpdateSkins(int playerId, string skins)
    {
        var payload = new SkinPayload { id = playerId.ToString(), skins = skins };
        string jsonPayload = JsonUtility.ToJson(payload);

        if (string.IsNullOrEmpty(jsonPayload))
        {
            Debug.LogError("Failed to serialize payload to JSON.");
            return;
        }

        using UnityWebRequest request = new UnityWebRequest(api_url + "skins", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("SkinList uploaded successfully");
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

[System.Serializable]
public class Skin
{
    public string skin;
}

[System.Serializable]
public class SkinPayload
{
    public string id;
    public string skins;
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