using UnityEngine;

[System.Serializable]
public class User : MonoBehaviour
{
    public static User Instance;
    
    public int id;
    public string nickname;
    public string email;
    public string password;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
