using System.Runtime.InteropServices;
using UnityEngine;

public class UnityToJSTest : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CallJSFunction(string message);

    public void OnClick(string message)
    {   
#if UNITY_WEBGL && !UNITY_EDITOR
        CallJSFunction(message);
#endif
    }
}
