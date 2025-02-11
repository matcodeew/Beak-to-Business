using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    private GameObject _baseFade;
    
    public void setBase(GameObject _base)
    {
        _baseFade = _base;
    }

    public void Fade(GameObject _target)
    {
        StartCoroutine(Fade1To2(_baseFade, _target));
    }

    public IEnumerator Fade1To2(GameObject _base, GameObject _target)
    {
        float _time = 0;

        while (_time < 0.5)
        {
            _base.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, _time/0.5f);
            _time += Time.deltaTime;
            yield return null;
        }

        _base.GetComponent<CanvasGroup>().alpha = 0;
        
        _base.SetActive(false);
        _target.SetActive(true);
        
        _time = 0;
        
        while (_time < 0.5)
        {
            _target.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, _time/0.5f);
            _time += Time.deltaTime;
            yield return null;
        }

        _target.GetComponent<CanvasGroup>().alpha = 1;
    }
}
