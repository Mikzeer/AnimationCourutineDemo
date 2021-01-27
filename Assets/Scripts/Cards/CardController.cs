using CommandPatternActions;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class CardController : AbilityManager
    {
        Dictionary<int, Card> allCards = new Dictionary<int, Card>();
        int cardIndex = 0;
        InGameCardCollectionManager inGameCardCollectionManager;
        bool debugOn = false;
        CardManagerUI cardManagerUI;

        public CardController(InGameCardCollectionManager cardCollectionManager, CardManagerUI cardManagerUI)
        {
            this.inGameCardCollectionManager = cardCollectionManager;
            this.cardManagerUI = cardManagerUI;
        }

        public void LoadDeckFromConfigurationData(Player player, ConfigurationData cnfDat)
        {
            // TENEMOS QUE CARGAR AL CARD COLLECTION MANAGER DESDE ALGUN LUGAR
            Deck userDeck = cnfDat.selectedDeck;
            List<DefaultCollectionDataDB> dfColl = userDeck.userDeckJson;
            List<Card> cardsOnDeck = new List<Card>();
            for (int i = 0; i < dfColl.Count; i++)
            {
                for (int x = 0; x < dfColl[i].Amount; x++)
                {
                    CardData cardData = inGameCardCollectionManager.GetCardDataByCardID(dfColl[i].ID);
                    Card card = new Card(cardIndex, player, cardData);
                    cardIndex++;
                    cardsOnDeck.Add(card);
                }

            }
            // ACA DEBERIAMOS CHEQUEAR QUE NUESTRO MAZO SEA VALIDO 
            Shuffle(cardsOnDeck);
            player.Deck = new Stack<Card>(cardsOnDeck);
        }

        private void Shuffle(List<Card> deck)
        {
            var r = new System.Random();
            for (int n = deck.Count - 1; n > 0; n--)
            {
                var k = r.Next(n + 1);
                var temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
        }

        public bool CanITakeACard(Player player)
        {
            if (!IsLegalTakeCard(player))
            {
                Debug.Log("Ilegal Take Card");
                return false;
            }

            if (player.Abilities.ContainsKey(ABILITYTYPE.TAKEACARD) == false)
            {
                if (debugOn) Debug.Log("El Player no tiene la Take Card Ability");
                return false;
            }

            TakeCardAbility takceCardAbility = (TakeCardAbility)player.Abilities[ABILITYTYPE.TAKEACARD];
            if (takceCardAbility == null)
            {
                if (debugOn) Debug.Log("La ID de la TakeCard Ability puede estar mal no funciono el casteo");
                return false;
            }

            return true;
        }

        private bool IsLegalTakeCard(Player player)
        {
            // 2- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            // QUIEN QUIERE LEVANTAR CARD
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // ENTONCES EL SERVER TE DICE SI, PODES LEVANTAR CARD
            return true;
        }

        // ESTO VA A SER UN COMMAND
        private Card TakeCardFromDeck(Player player)
        {
            Card card = player.Deck.Pop();
            allCards.Add(card.IDInGame, card);
            return card;
        }

        public void OnTakeCard(Player player)
        {
            TakeCardAbility takceCardAbility = (TakeCardAbility)player.Abilities[ABILITYTYPE.TAKEACARD];
            TakeCardAbilityEventInfo tkeCardInfo = new TakeCardAbilityEventInfo(player, TakeCardFromDeck(player), cardIndex);
            takceCardAbility.SetRequireGameData(tkeCardInfo);
            StartPerform(takceCardAbility);
            if (takceCardAbility.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }
            AddCard(tkeCardInfo);
            Perform(takceCardAbility);
            EndPerform(takceCardAbility);
            cardIndex++;
        }

        private void AddCard(TakeCardAbilityEventInfo tkeCardInfo)
        {
            IAddCardCommand addCardCmd = new IAddCardCommand(tkeCardInfo.cardTaker, tkeCardInfo.card);
            Invoker.AddNewCommand(addCardCmd);
            Invoker.ExecuteCommands();

            GameObject cardGo = cardManagerUI.CreateNewCardPrefab(tkeCardInfo.card, tkeCardInfo.cardTaker.OwnerPlayerID, OnTryUseCard);
            Motion takeCardMotion = cardManagerUI.AddCard(cardGo, tkeCardInfo.cardTaker.PlayerID);
            InvokerMotion.AddNewMotion(takeCardMotion);
            InvokerMotion.StartExecution(cardManagerUI);
        }

        public void SendCardToGraveyard(TakeCardAbilityEventInfo tkeCardInfo, RectTransform cardRect)
        {
            ISendCardToGraveyardCommand destroyCardCmd = new ISendCardToGraveyardCommand(tkeCardInfo.cardTaker, tkeCardInfo.card);
            Invoker.AddNewCommand(destroyCardCmd);
            Invoker.ExecuteCommands();

            Motion sendToGraveyardMotion = cardManagerUI.OnCardSendToGraveyard(cardRect, tkeCardInfo.cardTaker.PlayerID);
            InvokerMotion.AddNewMotion(sendToGraveyardMotion);
            InvokerMotion.StartExecution(cardManagerUI);
        }

        public void OnCardWaitForTargetSelection(Transform cardTransform)
        {
            Motion twaitMotion = cardManagerUI.OnCardWaitingTarget(cardTransform);
            InvokerMotion.AddNewMotion(twaitMotion);
            InvokerMotion.StartExecution(cardManagerUI);
        }

        public void OnTryUseCard(CardInGameUINEW toUseCard)
        {
            Debug.Log("I TRY TO USE CARD " + toUseCard.gameObject.name);
        }
    }
}