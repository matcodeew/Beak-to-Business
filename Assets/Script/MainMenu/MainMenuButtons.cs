using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite baseSprite;
    public Sprite hoverSprite;

    private Image _sprite;

    private void Start()
    {
        _sprite = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sprite.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _sprite.sprite = baseSprite;
    }
}
