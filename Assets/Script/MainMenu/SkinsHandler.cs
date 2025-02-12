using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinsHandler : MonoBehaviour
{
    private Skin _testUser;

    public Button skinButton;

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
    
    public TextMeshProUGUI feedbackText;
    public List<VertexGradient> gradients;
    
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

    public void CheckSkin() //Checks the skins the player has
    {
        StartCoroutine(CheckSkins());
    }
    
    public void ChangeSelectedSkin() //Applies skin when pressing 'select' in the skin menu
    {
        if (_skins[skinsNames[_previewNb]])
        {
            skinSelected = _previewNb;
            StartCoroutine(FeedBackText("Skin selected !", 1, true));
            return;
        }
        
        StartCoroutine(FeedBackText("You don't own that skin !", 1, false));
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
            StartCoroutine(FeedBackText("Hat selected !", 1, true));
            return;
        }
        
        StartCoroutine(FeedBackText("You don't own that hat !", 1, false));
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
    
    public IEnumerator CheckSkins()
    {
        var _task = APICaller.GetSkinById(_userId);
        yield return new WaitUntil(() => _task.IsCompleted);
        _testUser = _task.Result;
        
        
        string[] _userSkins = _testUser.skin.Split(',');
        
    
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
            
        }
        
        ChangePreviewHat(hatSelected);
        ChangePreviewSkin(skinSelected);
    }
    
    private IEnumerator FeedBackText(string _message, int _animTime, bool _good)
    {
        if (_good)
        {
            feedbackText.colorGradient = gradients[1];
        }
        else
        {
            feedbackText.colorGradient = gradients[0];
        }
        
        float _time = 0;
        Transform _feedbackTransform = feedbackText.gameObject.transform;
        
        Vector2 _feedbackPosition = _feedbackTransform.localPosition;
        
        feedbackText.text = _message;

        while (_time < _animTime)
        {
            feedbackText.alpha = Mathf.Lerp(0f, 1f, _time / _animTime);
            _feedbackTransform.localPosition = Vector3.Lerp(_feedbackTransform.localPosition,
                                                        (Vector2)_feedbackTransform.localPosition + Vector2.up * _time,
                                                         _time / _animTime);
            _time += Time.deltaTime;
            yield return null;
        }
        feedbackText.alpha = 1;
        _time = 0;

        while (_time < _animTime)
        {
            feedbackText.alpha = Mathf.Lerp(1f, 0f, _time / _animTime);
            _feedbackTransform.localPosition = Vector3.Lerp(_feedbackTransform.localPosition,
                (Vector2)_feedbackTransform.localPosition + Vector2.up * _time,
                _time / _animTime);
            _time += Time.deltaTime;
            yield return null;
        }
        
        feedbackText.alpha = 0;
        _feedbackTransform.localPosition = _feedbackPosition;
    }

    public void UpdateSkins(string _skin)
    {
        APICaller.UpdateSkins(_userId, _skin);
    }
}
