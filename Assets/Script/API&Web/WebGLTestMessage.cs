using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class WebGLTestMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    private int playerID;
    
    [DllImport("__Internal")]
    private static extern void GetPlayerIdCookie();
    
    public void TestAction(string message)
    {
        transform.Rotate(0, 0, 90);
        debugText.text = message;
        Debug.Log("TestAction: " + message);
    }

    public void ColorAction()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.value,
            UnityEngine.Random.value, UnityEngine.Random.value);
    }
    private void Start()
    {
        gameObject.name = "WebGLBridge";
#if UNITY_WEBGL && !UNITY_EDITOR
        GetPlayerIdCookie();
#endif
    }
    
    public void SetPlayerID(string id)
    {
        int parseId = int.Parse(id);
        StartCoroutine(InitUser(parseId));
        Debug.Log("Searching id..." + parseId);
    }
    
    private IEnumerator InitUser(int id)
    {
        var
            task = APICaller
                .GetUserById(
                    id); // This is better as it makes implementation easier, if problem ever rise you can just change it to TestApiCaller() to test
        yield return new WaitUntil(() => task.IsCompleted);
        playerID = task.Result.id;
        Debug.Log("ID found !");
        debugText.text = "id :" + playerID;
        Debug.Log("id :" + playerID + "player name :" + task.Result.nickname + "player email :" + task.Result.email + "player password :" + task.Result.password);
    }
    
}
