using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RibbonManager
{
    Dictionary<string, RiboonDisplay> ribbons;
    RibbonManagerUI ribbonManagerUI;

    public RibbonManager(RibbonManagerUI ribbonManagerUI)
    {
        this.ribbonManagerUI = ribbonManagerUI;
        ribbons = new Dictionary<string, RiboonDisplay>();
    }

    public void AddRibbon(RibbonData ribbonData, DeckBuilderCreationManager auxDeck)
    {
        if (ribbons.ContainsKey(ribbonData.name))
        {
            ribbons[ribbonData.name].AddAmount();
        }
        else
        {
            RiboonDisplay ribbonDisplay = ribbonManagerUI.CreateNewCardRibbon();
            ribbonDisplay.SetData(ribbonData, auxDeck.OnTryRemoveRibbonCardFromDeck, ribbonManagerUI.GetCardDisplay());
            ribbons.Add(ribbonData.name, ribbonDisplay);
        }
    }

    public void RemoveRibbon(RibbonData ribbonData)
    {
        if (ribbons.ContainsKey(ribbonData.name))
        {
            GameObject.Destroy(ribbons[ribbonData.name].gameObject);
            ribbons.Remove(ribbonData.name);
            ribbonManagerUI.GetCardDisplay().gameObject.SetActive(false);
        }
    }

    public void RestRibbon(RibbonData ribbonData)
    {
        if (ribbons.ContainsKey(ribbonData.name))
        {
            ribbons[ribbonData.name].RestAmount();
        }
    }

    public void Clear()
    {
        for (int i = ribbons.Count - 1; i >= 0; i--)
        {
            var item = ribbons.ElementAt(i);
            //var itemKey = item.Key;
            var itemValue = item.Value;
            GameObject.Destroy(itemValue.gameObject);
        }
        ribbons.Clear();
    }
}
