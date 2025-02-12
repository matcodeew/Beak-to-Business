using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinsHandler : MonoBehaviour
{
    private Skin _testUser;

    private Dictionary<string, bool> _skins = new();
    private Dictionary<string, bool> _hats = new();
    public List<string> skinsNames;
    public List<string> hatNames;
    
    public List<Sprite> skinsSprites;
    public List<Sprite> hatSprites;
    public int skinSelected;
    public int hatSelected;

    public Image lilPreview; 
    public Image bigPreview; 
    public TextMeshProUGUI skinsNumber;
    
    public Image hatLilPreview; 
    public Image hatBigPreview; 
    public TextMeshProUGUI hatsNumber;
    
    private int _previewNb;
    
    public MainMenuScript mainMenuScript;

    private int _userId;
    
    private void Start()
    {
        for (int i = 0; i < skinsNames.Count; i++)
        {
            _skins.Add(skinsNames[i], false);
        }

        for (int i = 0; i < hatNames.Count; i++)
        {
            _hats.Add(hatNames[i], false);
        }
        
        mainMenuScript._onUserLoggedIn.AddListener(GetId);
    }
    public void GetId(int _id)
    {
        _userId = _id;
    }

    public void CheckSkin()
    {
        StartCoroutine(CheckSkins());
    }

    public IEnumerator CheckSkins()
    {
        Debug.Log("CheckingSkin");
        var _task = APICaller.GetSkinById(_userId);
        yield return new WaitUntil(() => _task.IsCompleted);
        _testUser = _task.Result;
        
        Debug.Log("Checked");
        
        string[] _userSkins = _testUser.skin.Split(',');
        
        Debug.Log("Splitted");
    
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
        
        Debug.Log("GaveSkin");
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

        if (_skins[skinsNames[_previewNb]])
        {
            Debug.Log("you have skin !");
            lilPreview.color = bigPreview.color = Color.white;
        }
        else
        {
            Debug.Log("you dont have skin !");
            lilPreview.color = bigPreview.color = Color.grey;
        }
    }
    
    public void ChangeSelectedHat()
    {
        if (_hats[hatNames[_previewNb]])
        {
            hatSelected = _previewNb;
            Debug.Log("hat Selected !");
            return;
        }
        
        Debug.Log("You don't own this hat !");
    }
    
    public void ChangePreviewHat(int _preview)
    {
        _previewNb = _preview;
        hatLilPreview.sprite = hatBigPreview.sprite = hatSprites[_preview];
        hatsNumber.text = _preview.ToString();
        
        if (_hats[hatNames[_previewNb]])
        {
            Debug.Log("you have hat !");
            hatLilPreview.color = hatBigPreview.color = Color.white;
        }
        else
        {
            Debug.Log("you dont have hat !");
            hatLilPreview.color = hatBigPreview.color = Color.grey;
        }
    }

    public void UpdateSkins(string _skin)
    {
        
    }
}
