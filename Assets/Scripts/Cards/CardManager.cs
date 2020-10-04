using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardManager
    {
        Dictionary<int, Card> allCards = new Dictionary<int, Card>();
        int allCardIndex = 0;

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


        MotionController motionControllerCardSpawn = new MotionController();
        private int cardIndex = 0;
        public void AddCard(Player player)
        {
            // CUANDO LEVANTAMOS UNA CARTA CHEQUEAR
            // SI ES AUTOMATICA HAY QUE JUGARLA INMEDIATAMANETE, NO VA A IR A LA MANO
            // SI NO TIENE NINGUN TARGET POSIBLE VA AL CEMENTERIO
            // DE TENER TARGET:
            // SI RequireSelectTarget ES FALSO SE APLICA EL EFECTO INMEDIATAMENTE SIN SELECCIONAR AL/LOS TARGET/S
            // SI RequireSelectTarget ES VERDADERO ENTONCES HAY QUE ESPERAR A QUE SELECCIONE UNO O VARIOS TARGETS
            // SI ES AUTOMATICA Y TIENE TARGETS VAMOS A ESTAR OBLIGADOS A SELECCIONARLOS
            // SI NO ES AUTOMATICA Y TIENE TARGETS Y SELECCIONAMOS ALGUNO INVALIDO, LA CARTA VUELVE A LA MANO

            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                // instanciar el prefab de la card y ponerle como parent el canvas
                // ponerlo en el centro de la pantalla
                Vector3 screenCenter = Helper.GetCameraCenterWorldPositionWithZoffset();
                GameObject createdCardGameObject = GameObject.Instantiate(AnimotionHandler.Instance.cardUIPrefab, screenCenter, Quaternion.identity, AnimotionHandler.Instance.canvasRootTransform);

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
                    cardUI.SetPlayerHandTransform(AnimotionHandler.Instance.playersOneHand, AnimotionHandler.Instance.FireCardUITest, AnimotionHandler.Instance.infoPanel.SetText, AnimotionHandler.Instance.SetActiveInfoCard);
                    cardUI.SetRealCardDrop(newCard.OnDropCard, newCard.ID);

                    // generar una action que se active cuando dropeas la carta en la zona DropeableArea
                    // la Action<CardUI> es la que vamos a disparar desde el AnimotionHandler
                    // CardUI dropea, si hitea con dropeable es ahi donde disparamos el EVENT
                }

                Vector3 normalScale = createdCardGameObject.transform.localScale;
                Vector3 startScale = new Vector3(0.2f, 0.2f, 1);
                createdCardGameObject.transform.localScale = startScale;

                // hacerla animacion de instanciamiento a la mano y saliendo un poco de la pantalla

                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                RectTransform cardRect = createdCardGameObject.GetComponent<RectTransform>();
                PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(AnimotionHandler.Instance, cardRect, 1, normalScale, 1);
                motionsSpawn.Add(motionTweenScaleUp);

                Vector3 finalCardPosition = AnimotionHandler.Instance.playersOneHand.GetChild(AnimotionHandler.Instance.playersOneHand.childCount - 1).position;

                PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(AnimotionHandler.Instance, createdCardGameObject.transform, 1, finalCardPosition, 2);
                motionsSpawn.Add(motionTweenSpawn);

                // tengo que setear el parent
                List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, AnimotionHandler.Instance.playersOneHand, 3);
                configurables.Add(cardHandSetParentConfigAnimotion);

                CombineMotion combineMoveMotion = new CombineMotion(AnimotionHandler.Instance, 1, motionsSpawn, configurables);

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

            if (allCards.ContainsKey(cardUI.ID))
            {

            }

            //OnCardWaitingTarget(cardUI);
            //OnCardSendToGraveyard(cardUI);

            bool isSapwn = false;

            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                List<PositionerDemo.Motion> motionsWaitToGraveyard = new List<PositionerDemo.Motion>();

                PositionerDemo.Motion motionTweenToCardWaitPosition = new SpawnCardTweenMotion(AnimotionHandler.Instance, cardUI.transform, 1, AnimotionHandler.Instance.cardWaitPosition.position, 2);
                motionsWaitToGraveyard.Add(motionTweenToCardWaitPosition);

                PositionerDemo.Motion motionTimer = new TimeMotion(AnimotionHandler.Instance, 2, 2f);
                motionsWaitToGraveyard.Add(motionTimer);

                List<PositionerDemo.Configurable> configurables = new List<Configurable>();

                if (isSapwn)
                {
                    Vector3 finalCardPosition = AnimotionHandler.Instance.playersOneHand.GetChild(AnimotionHandler.Instance.playersOneHand.childCount - 1).position;
                    PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(AnimotionHandler.Instance, cardUI.transform, 3, finalCardPosition, 2);
                    motionsWaitToGraveyard.Add(motionTweenSpawn);

                    // tengo que setear el parent
                    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentHandConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, AnimotionHandler.Instance.playersOneHand, 4);
                    configurables.Add(cardHandSetParentHandConfigAnimotion);

                    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, AnimotionHandler.Instance.playerOneGraveyard, 4);
                    configurables.Add(blockRayCastConfigAnimotion);
                }
                else
                {
                    Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);
                    RectTransform cardRect = cardUI.GetComponent<RectTransform>();
                    PositionerDemo.Motion motionTweenScaleDownToGraveyard = new ScaleRectTweenMotion(AnimotionHandler.Instance, cardRect, 3, finalScale, 1);
                    motionsWaitToGraveyard.Add(motionTweenScaleDownToGraveyard);

                    Vector3 finalCardPositionGraveyard = AnimotionHandler.Instance.playerOneGraveyardLogo.position;
                    PositionerDemo.Motion motionTweenCardToGraveyard = new SpawnCardTweenMotion(AnimotionHandler.Instance, cardUI.transform, 3, finalCardPositionGraveyard, 2);
                    motionsWaitToGraveyard.Add(motionTweenCardToGraveyard);

                    // tengo que setear el parent
                    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentGraveyardConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, AnimotionHandler.Instance.playerOneGraveyard, 4);
                    configurables.Add(cardHandSetParentGraveyardConfigAnimotion);

                    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, AnimotionHandler.Instance.playerOneGraveyard, 4);
                    configurables.Add(blockRayCastConfigAnimotion);

                }

                CombineMotion combineMoveMotion = new CombineMotion(AnimotionHandler.Instance, 1, motionsWaitToGraveyard, configurables);

                motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
                motionControllerCardSpawn.TryReproduceMotion();
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


            AnimotionHandler.Instance.infoPanel.SetActive(isActive);
        }

    }
}