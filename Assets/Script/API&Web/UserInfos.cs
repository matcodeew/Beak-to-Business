using UnityEngine;

[System.Serializable]
public class UserInfos : MonoBehaviour
{
    public static UserInfos Instance;
    
    public int id;
    public string nickname;
    public string email;
    public string password;

    public int selectedSkin;

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
