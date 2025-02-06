using System;
using TMPro;
using UnityEngine;

public class WebGLTestMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    public void TestAction(string message)
    {
        transform.Rotate(0, 0, 90);
        debugText.text = message;
        Debug.Log("TestAction: " + message);
    }

    public void ColorAction()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
    private void Start()
    {
        TestAction("a");
        gameObject.name = "WebGLBridge";
    }
    
    
}
