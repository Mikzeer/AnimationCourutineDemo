﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardManager
    {
        Dictionary<int, Card> allCards = new Dictionary<int, Card>();
        int allCardIndex = 0;
        MotionController motionControllerCardSpawn = new MotionController();
        private int cardIndex = 0;

        public void CreateDeck(Player player, List<CardScriptableObject> cardSOList)
        {
            //Sprite[] miniatureSprite = Resources.LoadAll<Sprite>("CardMiniatureSprite");
            List<Card> cardsOnDeck = new List<Card>();

            for (int i = 0; i < cardSOList.Count; i++)
            {
                Card card = new Card(cardSOList[i].ID, player, cardSOList[i]);
                cardsOnDeck.Add(card);
            }

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

        private bool AreThereCardAvailables(Player player)
        {
            if (player.Deck.Count > 0) return true;

            return false;
        }

        private bool IsLegalTakeCard(Player player)
        {
            return true;
        }

        private Card TakeCardFromDeck(Player player)
        {
            if (AreThereCardAvailables(player) == false) return null;

            Card card = player.Deck.Pop();
            allCards.Add(allCardIndex, card);
            allCardIndex++;
            return card;
            //return player.Deck.Pop();
        }

        public void MoveCardFromHandToGraveyard(Card card, Player player)
        {
            player.PlayersHands.Remove(card);
            player.Graveyard.Add(card);
        }

        public void OnTryTakeCard(Player player)
        {
            if (motionControllerCardSpawn == null)
            {
                Debug.Log("No Motion Controller");
                return;
            }

            if (motionControllerCardSpawn.isPerforming == true)
            {
                Debug.Log("Is Performing animation");
                return;
            }

            // 2- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            // QUIEN QUIERE LEVANTAR CARD
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // ENTONCES EL SERVER TE DICE SI, PODES LEVANTAR CARD
            if (!IsLegalTakeCard(player))
            {
                Debug.Log("Ilegal Take Card");
                return;
            }

            TakeCardAbility takceCardAbility = null;
            if (player.Abilities.ContainsKey(1))
            {

                takceCardAbility = (TakeCardAbility)player.Abilities[1];
            }
            else
            {
                Debug.Log("El Player no tiene la Spawn Ability");
            }

            if (takceCardAbility == null)
            {
                Debug.Log("La ID de la Spawn Ability puede estar mal no funciono el casteo");
                return;
            }

            if (takceCardAbility.OnTryExecute() == false)
            {
                Debug.Log("Fallo en el On Try Execte de la Spawn Ability");
                return;
            }
            else
            {
                takceCardAbility.Perform();
            }

            if (takceCardAbility.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED)
            {
                Debug.Log("Se Cancelo desde la Ability la Spawn Ability");
                return;
            }

            // SI ESTOY EN EL JUEGO NORMAL TAKECARD NORMALMENTE
            // SI ESTOY EN EL JUEGO ONLINE ENTONCES EL TAKECARD SE VA A ENCARGAR EL SERVER
            // YA QUE NO SOLO LO TENGO QUE HACER INTERNAMENTE, SINO QUE TAMBIEN LO TIENE QUE VER REFLEJADO 
            // EL OTRO JUGADOR

            // El TakeCardEventInfo podria tener una List<Motion>... esas motions las agregaria cada uno de 
            // los modifiers que se le aplicaron al player al momento de TakeCard, y se ejecutarian con algun index dentro de la animacion de TakeCard
            // o en el mismo momento que empieza indicando todas las modificaciones.
            // Cada AbilityModifier en si se encargaria de llenar la lista del TakeCardEventInfo.List<Motions> ya que cada uno
            // sabria que animacion aplicar al momento de activar un efecto, al igual que el sonido y demas.

            AddCard(player);
        }

        public void AddCard(Player player)
        {
            // CUANDO LEVANTAMOS UNA CARTA CHEQUEAR
            // SI ES AUTOMATICA HAY QUE JUGARLA INMEDIATAMANETE, NO VA A IR A LA MANO
            // SI NO TIENE NINGUN TARGET POSIBLE VA AL CEMENTERIO
            // DE TENER TARGET:
            // SI RequireSelectTarget ES FALSO SE APLICA EL EFECTO INMEDIATAMENTE SIN SELECCIONAR AL/LOS TARGET/S
            // SI RequireSelectTarget ES VERDADERO ENTONCES HAY QUE ESPERAR A QUE SELECCIONE UNO O VARIOS TARGETS
            // SI ES AUTOMATICA Y TIENE TARGETS VAMOS A ESTAR OBLIGADOS A SELECCIONARLOS
            // SI ES AUTOMATICA Y NO TIENE TARGETS ENTONCES VAMOS A USARLA AUTOMATICAMENTE

            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                // instanciar el prefab de la card y ponerle como parent el canvas
                // ponerlo en el centro de la pantalla
                Vector3 screenCenter = Helper.GetCameraCenterWorldPositionWithZoffset();
                GameObject createdCardGameObject = GameObject.Instantiate(GameCreator.Instance.cardUIPrefab, screenCenter, Quaternion.identity, GameCreator.Instance.canvasRootTransform);

                createdCardGameObject.name = "CARD N " + cardIndex;
                cardIndex++;
                createdCardGameObject.GetComponentInChildren<Text>().text = createdCardGameObject.name;
                MikzeerGame.CardUI cardUI = createdCardGameObject.GetComponent<MikzeerGame.CardUI>();


                Card newCard = TakeCardFromDeck(player);
                player.PlayersHands.Add(newCard);
                MikzeerGame.CardDisplay cardDisplay = createdCardGameObject.GetComponent<MikzeerGame.CardDisplay>();

                if (cardDisplay != null)
                {
                    cardDisplay.Initialized(newCard);
                }

                if (cardUI != null)
                {
                    if (player.PlayerID == 0)
                    {
                        cardUI.SetPlayerHandTransform(GameCreator.Instance.cardHolderP1, FireCardUITest, GameCreator.Instance.infoPanel.SetText, SetActiveInfoCard);
                    }
                    else
                    {
                        cardUI.SetPlayerHandTransform(GameCreator.Instance.cardHolderP2, FireCardUITest, GameCreator.Instance.infoPanel.SetText, SetActiveInfoCard);
                    }

                    cardUI.SetRealCardDrop(newCard.OnDropCard, newCard.ID);

                    // generar una action que se active cuando dropeas la carta en la zona DropeableArea
                    // la Action<CardUI> es la que vamos a disparar desde el AnimotionHandler
                    // CardUI dropea, si hitea con dropeable es ahi donde disparamos el EVENT
                }

                Vector3 normalScale = createdCardGameObject.transform.localScale;
                Vector3 startScale = new Vector3(0.2f, 0.2f, 1);
                createdCardGameObject.transform.localScale = startScale;

                // hacerla animacion de instanciamiento a la mano y saliendo un poco de la pantalla

                List<Motion> motionsSpawn = new List<Motion>();
                RectTransform cardRect = createdCardGameObject.GetComponent<RectTransform>();
                Motion motionTweenScaleUp = new ScaleRectTweenMotion(GameCreator.Instance, cardRect, 1, normalScale, 1);
                motionsSpawn.Add(motionTweenScaleUp);

                Vector3 finalCardPosition = Vector3.zero;

                if (player.PlayerID == 0)
                {
                    finalCardPosition = GameCreator.Instance.cardHolderP1.GetChild(GameCreator.Instance.cardHolderP1.childCount - 1).position;
                }
                else
                {
                    finalCardPosition = GameCreator.Instance.cardHolderP2.GetChild(GameCreator.Instance.cardHolderP2.childCount - 1).position;
                }

                Motion motionTweenSpawn = new SpawnCardTweenMotion(GameCreator.Instance, createdCardGameObject.transform, 1, finalCardPosition, 2);
                motionsSpawn.Add(motionTweenSpawn);

                // tengo que setear el parent
                List<Configurable> configurables = new List<Configurable>();

                SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = null;

                if (player.PlayerID == 0)
                {
                    cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, GameCreator.Instance.cardHolderP1, 3);
                }
                else
                {
                    cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, GameCreator.Instance.cardHolderP2, 3);
                }

                configurables.Add(cardHandSetParentConfigAnimotion);

                AbilityActionStatusConfigureAnimotion<AbilityAction, Transform> abActionConfigureAnimotion = new AbilityActionStatusConfigureAnimotion<AbilityAction, Transform>(player.Abilities[1], 4);
                configurables.Add(abActionConfigureAnimotion);


                CombineMotion combineMoveMotion = new CombineMotion(GameCreator.Instance, 1, motionsSpawn, configurables);

                motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
                motionControllerCardSpawn.TryReproduceMotion();
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }

        public void FireCardUITest(MikzeerGame.CardUI cardUI)
        {
            Debug.Log("Se disparo el ON CARD USE");

            //if (allCards.ContainsKey(cardUI.ID))
            //{

            //}

            //OnCardWaitingTarget(cardUI);
            //OnCardSendToGraveyard(cardUI);

            bool isSapwn = false;

            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                //List<Motion> motionsWaitToGraveyard = new List<Motion>();

                //Motion motionTweenToCardWaitPosition = new SpawnCardTweenMotion(GameCreator.Instance, cardUI.transform, 1, GameCreator.Instance.cardWaitPosition.position, 2);
                //motionsWaitToGraveyard.Add(motionTweenToCardWaitPosition);

                //Motion motionTimer = new TimeMotion(GameCreator.Instance, 2, 2f);
                //motionsWaitToGraveyard.Add(motionTimer);

                //List<Configurable> configurables = new List<Configurable>();

                //if (isSapwn)
                //{
                //    Vector3 finalCardPosition = GameCreator.Instance.playersOneHand.GetChild(GameCreator.Instance.playersOneHand.childCount - 1).position;


                //    if (player.PlayerID == 0)
                //    {
                //        finalCardPosition = GameCreator.Instance.cardHolderP1.GetChild(GameCreator.Instance.cardHolderP1.childCount - 1).position;
                //    }
                //    else
                //    {
                //        finalCardPosition = GameCreator.Instance.cardHolderP2.GetChild(GameCreator.Instance.cardHolderP2.childCount - 1).position;
                //    }


                //    Motion motionTweenSpawn = new SpawnCardTweenMotion(GameCreator.Instance, cardUI.transform, 3, finalCardPosition, 2);
                //    motionsWaitToGraveyard.Add(motionTweenSpawn);

                //    // tengo que setear el parent
                //    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentHandConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, GameCreator.Instance.playersOneHand, 4);
                //    configurables.Add(cardHandSetParentHandConfigAnimotion);

                //    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, GameCreator.Instance.playerOneGraveyard, 4);
                //    configurables.Add(blockRayCastConfigAnimotion);
                //}
                //else
                //{
                //    Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);
                //    RectTransform cardRect = cardUI.GetComponent<RectTransform>();
                //    Motion motionTweenScaleDownToGraveyard = new ScaleRectTweenMotion(GameCreator.Instance, cardRect, 3, finalScale, 1);
                //    motionsWaitToGraveyard.Add(motionTweenScaleDownToGraveyard);

                //    Vector3 finalCardPositionGraveyard = GameCreator.Instance.playerOneGraveyardLogo.position;
                //    Motion motionTweenCardToGraveyard = new SpawnCardTweenMotion(GameCreator.Instance, cardUI.transform, 3, finalCardPositionGraveyard, 2);
                //    motionsWaitToGraveyard.Add(motionTweenCardToGraveyard);

                //    // tengo que setear el parent
                //    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentGraveyardConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, GameCreator.Instance.playerOneGraveyard, 4);
                //    configurables.Add(cardHandSetParentGraveyardConfigAnimotion);

                //    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, GameCreator.Instance.playerOneGraveyard, 4);
                //    configurables.Add(blockRayCastConfigAnimotion);

                //}

                //CombineMotion combineMoveMotion = new CombineMotion(GameCreator.Instance, 1, motionsWaitToGraveyard, configurables);

                //motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
                //motionControllerCardSpawn.TryReproduceMotion();
            }
        }

        public void SetActiveInfoCard(bool isActive)
        {
            if (isActive)
            {
                //Debug.Log("Animacion Card Apareciendo");
            }
            else
            {
                //Debug.Log("Animacion Card SE Va");
            }


            GameCreator.Instance.infoPanel.SetActive(isActive);
        }

    }
}