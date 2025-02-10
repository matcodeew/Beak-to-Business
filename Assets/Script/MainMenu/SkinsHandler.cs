using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinsHandler : MonoBehaviour
{
    private string _testUser;

    private Dictionary<string, bool> _skins = new();
    private Dictionary<string, bool> _hats = new();
    public List<string> skinsNames;
    
    public List<Sprite> skinsSprites;
    public int skinSelected;

    public Image lilPreview; 
    public Image bigPreview; 
    public TextMeshProUGUI skinsNumber;
    private int _previewNb;
    
    public MainMenuScript mainMenuScript;

    private int _userId;
    
    private void Awake()
    {
        for (int i = 0; i < skinsNames.Count; i++)
        {
            _skins.Add(skinsNames[i], false);
        }
        
        mainMenuScript._onUserLoggedIn.AddListener(GetId);
    }
    public void GetId(int _id)
    {
        _userId = _id;
        Debug.Log(_userId);
    }

    public void CheckSkin()
    {
        StartCoroutine(CheckSkins());
    }

    public IEnumerator CheckSkins()
    {
        var _task = APICaller.GetUserSkinById(_userId);
        yield return new WaitUntil(() => _task.IsCompleted);
        _testUser = _task.Result;
        
        Debug.Log(_testUser);
        
        string[] _userSkins = _testUser.Split(',');

        foreach (string _skin in _userSkins)
        {
            if (_skins.ContainsKey(_skin))
            {
                _skins[_skin] = true;
            }

            if (_hats.ContainsKey(_skin))
            {
                _hats[_skin] = true;
            }
            
            Debug.Log(_skin);
        }
    }

    public void ChangeSelectedSkin()
    {
        if (_skins[skinsNames[_previewNb]])
        {
            skinSelected = _previewNb;
            Debug.Log("skinSelected !");
            return;
        }
        
        Debug.Log("You don't own this skin !");
    }

    public void ChangePreviewSkin(int _preview)
    {
        _previewNb = _preview;
        lilPreview.sprite = bigPreview.sprite = skinsSprites[_preview];
        skinsNumber.text = _preview.ToString();
    }
}
