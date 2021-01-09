using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckSelectionUserDeckUI : MonoBehaviour
{
    Dictionary<int, UserDeckSelectionDisplay> userDeckSelectionDisplayDictionary;
    [SerializeField] private GameObject userDeckPrefab = default;
    [SerializeField] private RectTransform userDeckParent = default;

    public void ClearUserDeckDisplay()
    {
        if (userDeckSelectionDisplayDictionary == null) return;

        for (int i = userDeckSelectionDisplayDictionary.Count - 1; i >= 0; i--)
        {
            var item = userDeckSelectionDisplayDictionary.ElementAt(i);
            var itemValue = item.Value;
            Destroy(itemValue.gameObject);
        }
        userDeckSelectionDisplayDictionary.Clear();
    }

    public UserDeckSelectionDisplay CreateNewUserDeckDisplay(int ID)
    {
        if (userDeckSelectionDisplayDictionary == null)
        {
            userDeckSelectionDisplayDictionary = new Dictionary<int, UserDeckSelectionDisplay>();
        }
        GameObject deckPrefab = Instantiate(userDeckPrefab, userDeckParent);
        UserDeckSelectionDisplay userDeckDisplay = deckPrefab.GetComponent<UserDeckSelectionDisplay>();
        userDeckSelectionDisplayDictionary.Add(ID, userDeckDisplay);
        return userDeckDisplay;
    }
}