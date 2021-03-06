﻿using CommandPatternActions;
using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;
using StateMachinePattern;

namespace PositionerDemo
{
    public class CardController : AbilityManager
    {
        Dictionary<int, Card> allCards = new Dictionary<int, Card>();
        int cardIndex = 0;
        InGameCardCollectionManager inGameCardCollectionManager;
        bool debugOn = false;
        CardManagerUI cardManagerUI;
        CardTargetFiltterManager cardTargetFiltterManager;
        CardEffectManager cardEffectManager;
        GameMachine game;
        public CardController(InGameCardCollectionManager cardCollectionManager, CardManagerUI cardManagerUI, GameMachine game)
        {
            this.inGameCardCollectionManager = cardCollectionManager;
            this.cardManagerUI = cardManagerUI;
            cardTargetFiltterManager = new CardTargetFiltterManager(game.turnController, game.board2DManager);
            cardEffectManager = new CardEffectManager();
            this.game = game;
            CardPropertiesDatabase.GetCardSubClassByReflection();
        }

        #region LOAD AND CREATION

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
                    Card card = CardPropertiesDatabase.GetCardFromID(cardData.ID);
                    if (card == null)
                    {
                        continue;
                    }
                    card.InitializeCard(cardIndex, player, cardData);
                    card.SetCardState(CARDSTATES.DECK);
                    //Card card = new Card(cardIndex, player, cardData);
                    cardIndex++;
                    cardsOnDeck.Add(card);
                }

            }
            // ACA DEBERIAMOS CHEQUEAR QUE NUESTRO MAZO SEA VALIDO 
            Shuffle(cardsOnDeck);
            player.Deck = new Stack<Card>(cardsOnDeck);
        }

        public void LoadDeckTest(Player player)
        {
            List<Card> cardsOnDeck = new List<Card>();
            CardData buffUnitCardData = inGameCardCollectionManager.GetCardDataByCardID("CardID1");
            Card buffUnitCard = CardPropertiesDatabase.GetCardFromID(buffUnitCardData.ID);
            buffUnitCard.InitializeCard(cardIndex, player, buffUnitCardData);
            buffUnitCard.SetCardState(CARDSTATES.DECK);
            cardsOnDeck.Add(buffUnitCard);
            cardIndex++;

            CardData nerUnitCardData = inGameCardCollectionManager.GetCardDataByCardID("CardID8");
            Card nerfUnitCard = CardPropertiesDatabase.GetCardFromID(nerUnitCardData.ID);
            nerfUnitCard.InitializeCard(cardIndex, player, nerUnitCardData);
            nerfUnitCard.SetCardState(CARDSTATES.DECK);
            cardsOnDeck.Add(nerfUnitCard);
            cardIndex++;

            CardData healUnitCardData = inGameCardCollectionManager.GetCardDataByCardID("CardID4");
            Card healUnitCard = CardPropertiesDatabase.GetCardFromID(healUnitCardData.ID);
            healUnitCard.InitializeCard(cardIndex, player, healUnitCardData);
            healUnitCard.SetCardState(CARDSTATES.DECK);
            cardsOnDeck.Add(healUnitCard);
            cardIndex++;


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

        #endregion

        #region TAKE A CARD

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

            if (player.Deck.Count <= 0)
            {
                if (debugOn) Debug.Log("No Cards In Deck");
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
            if (player.Deck.Count <= 0)
            {
                return null;
            }
            Card card = player.Deck.Pop();
            allCards.Add(card.IDInGame, card);
            return card;
        }

        private Card PeekCardFromDeck(Player player)
        {
            if (player.Deck.Count <= 0)
            {
                return null;
            }
            Card card = player.Deck.Peek();
            return card;
        }

        public void OnTakeCard(Player player)
        {
            TakeCardAbility takceCardAbility = (TakeCardAbility)player.Abilities[ABILITYTYPE.TAKEACARD];
            TakeCardAbilityEventInfo tkeCardInfo = new TakeCardAbilityEventInfo(player, PeekCardFromDeck(player), PeekCardFromDeck(player).IDInGame);
            takceCardAbility.SetRequireGameData(tkeCardInfo);
            StartPerform(takceCardAbility);
            if (takceCardAbility.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }
            tkeCardInfo.card = TakeCardFromDeck(player);
            TakeCardFromDeck(tkeCardInfo);

            Perform(takceCardAbility); 


            EndPerform(takceCardAbility);
            //cardIndex++;
        }

        private void TakeCardFromDeck(TakeCardAbilityEventInfo tkeCardInfo)
        {
            IAddCardCommand addCardCmd = new IAddCardCommand(tkeCardInfo.cardTaker, tkeCardInfo.card);
            Invoker.AddNewCommand(addCardCmd);
            Invoker.ExecuteCommands();

            GameObject cardGo = cardManagerUI.CreateNewCardPrefab(tkeCardInfo.card, tkeCardInfo.cardTaker.OwnerPlayerID, this);
            //Motion takeCardMotion = cardManagerUI.AddCard(cardGo, tkeCardInfo.cardTaker.PlayerID);
            //InvokerMotion.AddNewMotion(takeCardMotion);
            //InvokerMotion.StartExecution(cardManagerUI);

            Animotion takeCardMotion = cardManagerUI.AddCardAnimotion(cardGo, tkeCardInfo.cardTaker.OwnerPlayerID);
            InvokerAnimotion.AddNewMotion(takeCardMotion);
            InvokerAnimotion.StartExecution(cardManagerUI);

            // ENTRAMOS EN ESTE ESTADO PARA PODER TOMAR LA CARD Y SOLO VER LA ANIMACION
            TakeCardState spawn = new TakeCardState(game, game.baseStateMachine.currentState);
            game.baseStateMachine.PopState(true);
            game.baseStateMachine.PushState(spawn);
        }

        #endregion

        #region GRAVEYARD

        public void SendCardToGraveyard(TakeCardAbilityEventInfo tkeCardInfo, RectTransform cardRect)
        {
            ISendCardToGraveyardCommand destroyCardCmd = new ISendCardToGraveyardCommand(tkeCardInfo.cardTaker, tkeCardInfo.card);
            Invoker.AddNewCommand(destroyCardCmd);
            Invoker.ExecuteCommands();

            //Motion sendToGraveyardMotion = cardManagerUI.OnCardSendToGraveyard(cardRect, tkeCardInfo.cardTaker.PlayerID);
            //InvokerMotion.AddNewMotion(sendToGraveyardMotion);
            //InvokerMotion.StartExecution(cardManagerUI);

            Animotion sendToGraveyardMotion = cardManagerUI.OnCardSendToGraveyardAnimotion(cardRect, tkeCardInfo.cardTaker.OwnerPlayerID);
            InvokerAnimotion.AddNewMotion(sendToGraveyardMotion);
            InvokerAnimotion.StartExecution(cardManagerUI);
        }

        #endregion

        #region TARGET SELECTION

        public void OnCardWaitForTargetSelection(Transform cardTransform)
        {
            //Motion twaitMotion = cardManagerUI.OnCardWaitingTarget(cardTransform);
            //InvokerMotion.AddNewMotion(twaitMotion);
            //InvokerMotion.StartExecution(cardManagerUI);

            Animotion twaitMotion = cardManagerUI.OnCardWaitingTargetAnimotion(cardTransform);
            InvokerAnimotion.AddNewMotion(twaitMotion);
            InvokerAnimotion.StartExecution(cardManagerUI);
        }

        public bool CanIEnterCardSelectionState(Player player, Card card)
        {
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR MANDA EL CMD SPAWN A LOS DOS JUGADORES

            // SI EL PLAYER ESTA EN SU TURNO
            if (player != game.turnController.CurrentPlayerTurn)
            {
                if (debugOn) Debug.Log("NO ES EL TURNO DEL PLAYER");
                return false;
            }

            List<ICardTarget> cardTargets = new List<ICardTarget>();

            cardTargets = cardTargetFiltterManager.OnTryGetFiltterTargets(card);

            if (cardTargets == null)
            {
                if (debugOn) Debug.Log("No Target Founds");
                return false;
            }


            //if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN) == false)
            //{
            //    if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
            //    return false;
            //}
            //SpawnAbility spw = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            //if (spw == null)
            //{
            //    if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
            //    return false;
            //}

            return true;
        }

        #endregion

        #region CARD USE

        public void OnTryUseCard(CardData toUseCard, RectTransform cardTransform)
        {
            Debug.Log("I TRY TO USE CARD " + toUseCard.CardName);
            //Animotion twaitMotion = cardManagerUI.OnCardWaitingTargetAnimotion(cardTransform);
            //InvokerAnimotion.AddNewMotion(twaitMotion);
            //InvokerAnimotion.StartExecution(cardManagerUI);
        }

        #endregion
    }
}