using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardManager
    {

        public void CreateDeck(Player player, List<CardScriptableObject> cardSOList)
        {
            //Sprite[] miniatureSprite = Resources.LoadAll<Sprite>("CardMiniatureSprite");

            List<Card> cardsOnDeck = new List<Card>();

            for (int i = 0; i < cardSOList.Count; i++)
            {
                Card card = new ShieldCard(cardSOList[i].ID, player, cardSOList[i]);
                cardsOnDeck.Add(card);
            }

            //Card card =  new Card("Attack +","Sube el ataque + 1", null, miniatureSprite[0]);
            //Card card2 = new Card("Life +", "Sube la vida + 1", null, miniatureSprite[1]);
            //Card card3 = new Card("Attack -", "Baja el ataque - 1", null, miniatureSprite[0]);
            //Card card4 = new Card("Life -", "Baja la vida - 1", null, miniatureSprite[1]);

            //Card card = new ShieldCard(null, miniatureSprite[0], actualPlayer, gameCreator);
            //Card card2 = new ShieldCard(null, miniatureSprite[0], actualPlayer, gameCreator);
            //Card card3 = new ShieldCard(null, miniatureSprite[0], actualPlayer, gameCreator);
            //Card card4 = new ShieldCard(null, miniatureSprite[0], actualPlayer, gameCreator);

            //cardsOnDeck.Add(card);
            //cardsOnDeck.Add(card2);
            //cardsOnDeck.Add(card3);
            //cardsOnDeck.Add(card4);

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

        public bool AreThereCardAvailables(Player player)
        {
            if (player.Deck.Count > 0) return true;
            //Debug.Log("No Card Available");
            return false;
        }

        public Card TakeCardFromDeck(Player player)
        {           
            return player.Deck.Pop();
        }

        public void MoveCardFromHandToGraveyard(Card card, Player player)
        {
            player.PlayersHands.Remove(card);
            player.Graveyard.Add(card);
        }


        MotionController motionControllerCardSpawn = new MotionController();
        private int cardIndex = 0;
        public void AddCard(Player player, AnimotionHandler animotionHandler)
        {
            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                // instanciar el prefab de la card y ponerle como parent el canvas
                // ponerlo en el centro de la pantalla
                Vector3 screenCenter = Helper.GetCameraCenterWorldPositionWithZoffset();
                GameObject createdCardGameObject = GameObject.Instantiate(animotionHandler.cardUIPrefab, screenCenter, Quaternion.identity, animotionHandler.canvasRootTransform);

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
                    cardUI.SetPlayerHandTransform(animotionHandler.playersOneHand, animotionHandler.FireCardUITest, animotionHandler.infoPanel.SetText, animotionHandler.SetActiveInfoCard);

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
                PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(animotionHandler, cardRect, 1, normalScale, 1);
                motionsSpawn.Add(motionTweenScaleUp);

                Vector3 finalCardPosition = animotionHandler.playersOneHand.GetChild(animotionHandler.playersOneHand.childCount - 1).position;

                PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(animotionHandler, createdCardGameObject.transform, 1, finalCardPosition, 2);
                motionsSpawn.Add(motionTweenSpawn);

                // tengo que setear el parent
                List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, animotionHandler.playersOneHand, 3);
                configurables.Add(cardHandSetParentConfigAnimotion);

                CombineMotion combineMoveMotion = new CombineMotion(animotionHandler, 1, motionsSpawn, configurables);

                motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
                motionControllerCardSpawn.TryReproduceMotion();
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }

        public void FireCardUITest(MikzeerGame.CardUI cardUI, AnimotionHandler animotionHandler)
        {
            Debug.Log("Se disparo el ON CARD USE");

            //OnCardWaitingTarget(cardUI);
            //OnCardSendToGraveyard(cardUI);

            bool isSapwn = false;

            if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
            {
                List<PositionerDemo.Motion> motionsWaitToGraveyard = new List<PositionerDemo.Motion>();

                PositionerDemo.Motion motionTweenToCardWaitPosition = new SpawnCardTweenMotion(animotionHandler, cardUI.transform, 1, animotionHandler.cardWaitPosition.position, 2);
                motionsWaitToGraveyard.Add(motionTweenToCardWaitPosition);

                PositionerDemo.Motion motionTimer = new TimeMotion(animotionHandler, 2, 2f);
                motionsWaitToGraveyard.Add(motionTimer);

                List<PositionerDemo.Configurable> configurables = new List<Configurable>();

                if (isSapwn)
                {
                    Vector3 finalCardPosition = animotionHandler.playersOneHand.GetChild(animotionHandler.playersOneHand.childCount - 1).position;
                    PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(animotionHandler, cardUI.transform, 3, finalCardPosition, 2);
                    motionsWaitToGraveyard.Add(motionTweenSpawn);

                    // tengo que setear el parent
                    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentHandConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, animotionHandler.playersOneHand, 4);
                    configurables.Add(cardHandSetParentHandConfigAnimotion);

                    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, animotionHandler.playerOneGraveyard, 4);
                    configurables.Add(blockRayCastConfigAnimotion);
                }
                else
                {
                    Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);
                    RectTransform cardRect = cardUI.GetComponent<RectTransform>();
                    PositionerDemo.Motion motionTweenScaleDownToGraveyard = new ScaleRectTweenMotion(animotionHandler, cardRect, 3, finalScale, 1);
                    motionsWaitToGraveyard.Add(motionTweenScaleDownToGraveyard);

                    Vector3 finalCardPositionGraveyard = animotionHandler.playerOneGraveyardLogo.position;
                    PositionerDemo.Motion motionTweenCardToGraveyard = new SpawnCardTweenMotion(animotionHandler, cardUI.transform, 3, finalCardPositionGraveyard, 2);
                    motionsWaitToGraveyard.Add(motionTweenCardToGraveyard);

                    // tengo que setear el parent
                    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentGraveyardConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, animotionHandler.playerOneGraveyard, 4);
                    configurables.Add(cardHandSetParentGraveyardConfigAnimotion);

                    SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, animotionHandler.playerOneGraveyard, 4);
                    configurables.Add(blockRayCastConfigAnimotion);

                }

                CombineMotion combineMoveMotion = new CombineMotion(animotionHandler, 1, motionsWaitToGraveyard, configurables);

                motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
                motionControllerCardSpawn.TryReproduceMotion();
            }
        }

        public void SetActiveInfoCard(bool isActive, AnimotionHandler animotionHandler)
        {
            if (isActive)
            {
                //Debug.Log("Animacion Card Apareciendo");
            }
            else
            {
                //Debug.Log("Animacion Card SE Va");
            }


            animotionHandler.infoPanel.SetActive(isActive);
        }

    }
}