using System;
using UnityEngine;

public class UserDeckSelectionDisplay : UserDeckDisplay
{
    Vector3 initialScale;
    Vector3 bigScale;
    float augScale = 1.1f;
    [SerializeField] private GameObject selectionImage = default;
    protected override void InitializeSimpleUI()
    {
        initialScale = uiRectTansform.localScale;
        bigScale = initialScale;
        bigScale.y *= augScale;
    }

    protected override void OnSuccesfulPointerEnter()
    {
        uiRectTansform.localScale = bigScale;
        selectionImage.SetActive(true);
    }

    protected override void OnSuccesfulPointerExit()
    {
        uiRectTansform.localScale = initialScale;
        selectionImage.SetActive(false);
    }
}