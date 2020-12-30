using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckBuilderUserDeckUI : MonoBehaviour
{
    Dictionary<int, UserDeckDisplay> userDeckDisplayDictionary;
    [SerializeField] private GameObject userDeckPrefab;
    [SerializeField] private RectTransform userDeckParent;

    public void ClearUserDeckDisplay()
    {
        if (userDeckDisplayDictionary == null) return;

        for (int i = userDeckDisplayDictionary.Count - 1; i >= 0; i--)
        {
            var item = userDeckDisplayDictionary.ElementAt(i);
            var itemValue = item.Value;
            Destroy(itemValue.gameObject);
        }
        userDeckDisplayDictionary.Clear();
    }

    public UserDeckDisplay CreateNewUserDeckDisplay(int ID)
    {
        if (userDeckDisplayDictionary == null)
        {
            userDeckDisplayDictionary = new Dictionary<int, UserDeckDisplay>();
        }
        GameObject deckPrefab = Instantiate(userDeckPrefab, userDeckParent);
        UserDeckDisplay userDeckDisplay = deckPrefab.GetComponent<UserDeckDisplay>();
        userDeckDisplayDictionary.Add(ID, userDeckDisplay);
        return userDeckDisplay;
    }

    public void ChangeUserDeckDisplayEvent(Action<Deck> OnDeckClick)
    {
        for (int i = userDeckDisplayDictionary.Count - 1; i >= 0; i--)
        {
            var item = userDeckDisplayDictionary.ElementAt(i);
            var itemValue = item.Value;
            itemValue.ChangeEvent(OnDeckClick);
        }
    }
}

