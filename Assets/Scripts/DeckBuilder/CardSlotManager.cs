using UnityEngine;

public class CardSlotManager
{
    int currentAddIndex = 0;
    bool fullIndex = false;
    CardSlotPage[] cardSlotPages;
    int currentPageIndex = 0;

    public CardSlotManager(int maxPages)
    {
        cardSlotPages = new CardSlotPage[maxPages];
    }

    public void AddCardSlotPage(CardSlotPage cardSlotPage)
    {
        if (fullIndex)
        {
            Debug.Log("FULL SLOT INDEX");
            return;
        }

        cardSlotPages[currentAddIndex] = cardSlotPage;

        currentAddIndex++;
        if (currentAddIndex > cardSlotPages.Length - 1)
        {
            fullIndex = true;
        }
    }

    public CardSlotPage GetCurrentPage()
    {
        return cardSlotPages[currentPageIndex];
    }

    public CardSlotPage NextPage()
    {
        currentPageIndex++;
        if (currentPageIndex > cardSlotPages.Length - 1) currentPageIndex = 0;

        return cardSlotPages[currentPageIndex];
    }

    public CardSlotPage PreviousPage()
    {
        currentPageIndex--;
        if (currentPageIndex < 0) currentPageIndex = cardSlotPages.Length - 1;

        return cardSlotPages[currentPageIndex];
    }

}
