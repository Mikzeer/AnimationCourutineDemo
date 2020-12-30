using MikzeerGame;
using PositionerDemo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserDeckDisplay : SimpleClickeableUI
{
    [SerializeField] protected Text txtName;
    protected Vector2 mousePos;
    protected Deck userDeck;
    public event Action<Deck> OnDeckClick;

    public void SetData(Deck userDeck, Action<Deck> OnDeckClick)
    {
        txtName.text = userDeck.name;
        this.userDeck = userDeck;
        this.OnDeckClick = OnDeckClick;
    }

    public void ChangeEvent(Action<Deck> OnDeckClick)
    {
        this.OnDeckClick = null;
        this.OnDeckClick = OnDeckClick;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Vector2 newMousePos = Input.mousePosition;
        if (mousePos != newMousePos) return;
        OnDeckClick?.Invoke(userDeck);
    }
}