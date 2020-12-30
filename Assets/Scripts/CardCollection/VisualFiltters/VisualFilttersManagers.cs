using PositionerDemo;
using System.Collections.Generic;

public class VisualFilttersManagers
{
    VisualFiltters vsFiltters;
    public bool showOnlyOwned = true;
    public VisualFilttersManagers()
    {
        vsFiltters = new VisualFiltters();
    }

    public void Filtter(VisualFiltters pVFiltters, Dictionary<CardData, CardDisplaySlot> cardSlotUIDisplay, bool showOnlyOwned)
    {
        this.showOnlyOwned = showOnlyOwned;
        vsFiltters.rarity = pVFiltters.rarity;
        vsFiltters.actType = pVFiltters.actType;
        vsFiltters.cType = pVFiltters.cType;
        vsFiltters.onlyChainable = pVFiltters.onlyChainable;
        vsFiltters.onlyDarkCards = pVFiltters.onlyDarkCards;
        vsFiltters.tagKeyword = pVFiltters.tagKeyword;
        vsFiltters.darkPoints = pVFiltters.darkPoints;
        foreach (KeyValuePair<CardData, CardDisplaySlot> csUI in cardSlotUIDisplay)
        {
            if (showOnlyOwned == true)
            {
                if (csUI.Value.cardSlotUI.cardSlot.userCollectionAmount <= 0)
                {
                    csUI.Value.cardSlotUI.SetActive(false);
                    continue;
                }
            }
            if (csUI.Key.CardRarity != pVFiltters.rarity && pVFiltters.rarity != CardRarity.NONE)
            {
                csUI.Value.cardSlotUI.SetActive(false);
                continue;
            }
            if (csUI.Key.CardType != pVFiltters.cType && pVFiltters.cType != CARDTYPE.NONE)
            {
                csUI.Value.cardSlotUI.SetActive(false);
                continue;
            }
            if (csUI.Key.ActivationType != pVFiltters.actType && pVFiltters.actType != ACTIVATIONTYPE.NONE)
            {
                csUI.Value.cardSlotUI.SetActive(false);
                continue;
            }
            if (pVFiltters.onlyChainable == true)
            {
                if (csUI.Key.IsChainable == false)
                {
                    csUI.Value.cardSlotUI.SetActive(false);
                    continue;
                }
            }
            if (pVFiltters.onlyDarkCards == true)
            {
                if (csUI.Key.IsDarkCard == false)
                {
                    csUI.Value.cardSlotUI.SetActive(false);
                    continue;
                }
            }
            if (pVFiltters.darkPoints > -1)
            {
                if (csUI.Key.DarkPoints != pVFiltters.darkPoints)
                {
                    csUI.Value.cardSlotUI.SetActive(false);
                    continue;
                }
            }
            if (pVFiltters.tagKeyword != null && pVFiltters.tagKeyword != "" && pVFiltters.tagKeyword != string.Empty)
            {
                bool sameKey = false;
                for (int i = 0; i < csUI.Key.Tags.Length; i++)
                {
                    if (csUI.Key.Tags[i] == pVFiltters.tagKeyword)
                    {
                        sameKey = true;
                    }
                }
                // THERE IS A KEYWORD TO SEARCH, BUT ITS NOT THE SAME, SO WE HIDE THE CARD
                if (sameKey == false)
                {
                    csUI.Value.cardSlotUI.SetActive(false);
                    continue;
                }
            }

            csUI.Value.cardSlotUI.SetActive(true);
        }
    }
}
