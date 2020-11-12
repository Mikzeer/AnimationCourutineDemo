using UnityEngine;

public class CardSlotPage
{
    CardSlot[,] cardSlots;
    CardSlotUI[,] cardSlotsUI;
    int pageIndex;

    public int rows = 2;
    public int columns = 4;
    int maxSlotsPerPage;

    int actualrow = 0;
    int actualcolumn = 0;

    public bool isFull = false;

    public CardSlotPage(int pageIndex)
    {
        this.pageIndex = pageIndex;
        cardSlots = new CardSlot[rows, columns];
        cardSlotsUI = new CardSlotUI[rows, columns];
        maxSlotsPerPage = rows * columns;
    }

    public void SetSlot(int rows, int columns, CardSlot cardSlot, CardSlotUI cardSlotUI)
    {
        if (rows < 0 || rows >= this.rows || columns < 0 || columns >= this.columns) return;

        cardSlots[rows, columns] = cardSlot;
        cardSlotsUI[rows, columns] = cardSlotUI;
    }

    public void AddNewSlot(CardSlot cardSlot, CardSlotUI cardSlotUI)
    {
        if (isFull)
        {
            return;
        }

        cardSlots[actualrow, actualcolumn] = cardSlot;
        cardSlotsUI[actualrow, actualcolumn] = cardSlotUI;

        actualcolumn++;
        if (actualcolumn > columns -1)
        {
            actualrow++;
            actualcolumn = 0;
            if (actualrow > rows - 1)
            {
                actualrow--;
                isFull = true;
                //Debug.Log("NO MORE SPACE IN PAGE");
                return;
            }
        }
    }

    public CardSlot GetSlot(int rows, int columns)
    {
        if (rows < 0 || rows >= this.rows || columns < 0 || columns >= this.columns) return null;

        return cardSlots[rows, columns];
    }

    public CardSlotUI GetSlotUI(int rows, int columns)
    {
        if (rows < 0 || rows >= this.rows || columns < 0 || columns >= this.columns) return null;

        return cardSlotsUI[rows, columns];
    }
}
