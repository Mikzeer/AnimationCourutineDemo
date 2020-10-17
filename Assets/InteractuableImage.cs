using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InteractuableImage : SimpleClickeableUI
{
    public Image backgroundImage;
    public Text text;
    private Color32 HighlightedColor = Color.blue;
    public Color32 UnactiveColor;
    private Color32 PressColor = Color.black;
    public bool IsPressed = false;

    protected override void InitializeSimpleUI()
    {
        UnactiveColor = backgroundImage.color;
    }

    protected override void OnSuccesfulPointerEnter()
    {
        if (IsPressed == true) return;


        backgroundImage.color = HighlightedColor;
    }

    protected override void OnSuccesfulPointerExit()
    {
        if (IsPressed == true) return;

        backgroundImage.color = UnactiveColor;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = !IsPressed;

        if (IsPressed)
        {
            backgroundImage.color = PressColor;
        }
    }

}
