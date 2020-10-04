using System.Collections.Generic;

namespace PositionerDemo
{
    public class CardDatabase
    {
        Dictionary<int, Card> cardDatabase;

        public CardDatabase()
        {

        }

        public Card GetCard(int cardIDSO)
        {
            if (cardDatabase.ContainsKey(cardIDSO))
            {
                return cardDatabase[cardIDSO];
            }

            return null;
        }

        private void CreateCardDatabase()
        {

            CardScriptableObject CardSO = new CardScriptableObject();
            //cardDatabase = new Dictionary<int, Card>()
            //{
            //    { 0, new ShieldCard (0, CardSO) },
            //    { 1, new ShieldCard (0, CardSO) }
            //};
        }
    }
}