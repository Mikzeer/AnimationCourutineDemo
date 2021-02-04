using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject cardUIPrefab = default;
        [SerializeField] private RectTransform cardHolderPlayerLeft = default;
        [SerializeField] private RectTransform cardHolderPlayerRight = default;
        [SerializeField] private RectTransform canvasRootTransform = default;
        [SerializeField] private InfoPanel infoPanel = default; // INFORMACION SOBRE HABILIDADES ESPECIALES DE LAS CARDS 
        [SerializeField] private RectTransform playerOneGraveyardLogo = default;
        [SerializeField] private RectTransform playerOneGraveyard = default;
        [SerializeField] private RectTransform playerTwoGraveyardLogo = default;
        [SerializeField] private RectTransform playerTwoGraveyard = default;
        [SerializeField] private Transform cardWaitPosition = default;
        [SerializeField] private CanvasScaler canvasScaler = default;
        public Motion AddCard(GameObject createdCardGameObject, int PlayerID)
        {                     
            Vector3 normalScale = createdCardGameObject.transform.localScale;
            Vector3 startScale = new Vector3(0.2f, 0.2f, 1);
            createdCardGameObject.transform.localScale = startScale;

            // hacerla animacion de instanciamiento a la mano y saliendo un poco de la pantalla
            List<Motion> motionsSpawn = new List<Motion>();
            RectTransform cardRect = createdCardGameObject.GetComponent<RectTransform>();
            Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, cardRect, 1, normalScale, 1);
            motionsSpawn.Add(motionTweenScaleUp);

            Vector3 finalCardPosition = Vector3.zero;
            if (PlayerID == 0)
            {
                if (cardHolderPlayerLeft.transform.childCount > 0)
                {
                    finalCardPosition = cardHolderPlayerLeft.GetChild(cardHolderPlayerLeft.childCount - 1).position;
                }
                else
                {
                    finalCardPosition = cardHolderPlayerLeft.position;
                }
            }
            else
            {
                if (cardHolderPlayerRight.transform.childCount > 0)
                {
                    finalCardPosition = cardHolderPlayerRight.GetChild(cardHolderPlayerRight.childCount - 1).position;
                }
                else
                {
                    finalCardPosition = cardHolderPlayerRight.position;
                }
            }

            Motion motionTweenSpawn = new SpawnCardTweenMotion(this, createdCardGameObject.transform, 1, finalCardPosition, 2);
            motionsSpawn.Add(motionTweenSpawn);

            // tengo que setear el parent
            List<Configurable> configurables = new List<Configurable>();

            SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = null;
            if (PlayerID == 0)
            {
                cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, cardHolderPlayerLeft, 3);
            }
            else
            {
                cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, cardHolderPlayerRight, 3);
            }
            configurables.Add(cardHandSetParentConfigAnimotion);

            CardInGameUINEW cardInGameUI = createdCardGameObject.GetComponent<CardInGameUINEW>();

            // activamos el canvas group para que pueda recibir raycast otra vez
            SetCanvasGroupBlockRaycastConfigureAnimotion<CardInGameUINEW, Transform> blockRayCastConfigAnimotion =
                new SetCanvasGroupBlockRaycastConfigureAnimotion<CardInGameUINEW, Transform>(cardInGameUI, null, 4);
            configurables.Add(blockRayCastConfigAnimotion);

            //Motion motionTimer = new TimeMotion(this, 2, 2f);
            //motionsWaitToGraveyard.Add(motionTimer);

            CombineMotion combineMoveMotion = new CombineMotion(this, 1, motionsSpawn, configurables);
            return combineMoveMotion;
        }

        public Motion OnCardSendToGraveyard(RectTransform cardRect, int PlayerID)
        {
            Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);

            List<Motion> motionsWaitToGraveyard = new List<Motion>();
            //RectTransform cardRect = cardUI.GetComponent<RectTransform>();
            Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, cardRect, 1, finalScale, 1);
            motionsWaitToGraveyard.Add(motionTweenScaleUp);

            Vector3 finalCardPosition = playerOneGraveyardLogo.position;
            if (PlayerID == 0)
            {
                finalCardPosition = playerOneGraveyardLogo.position; 
            }
            else
            {
                finalCardPosition = playerTwoGraveyardLogo.position;
            }

            Motion motionTweenSpawn = new SpawnCardTweenMotion(this, cardRect, 1, finalCardPosition, 2);
            motionsWaitToGraveyard.Add(motionTweenSpawn);

            // tengo que setear el parent
            List<Configurable> configurables = new List<Configurable>();

            RectTransform parentTransform = null;
            if (PlayerID == 0)
            {
                parentTransform = playerOneGraveyard;
            }
            else
            {
                parentTransform = playerTwoGraveyard;
            }

            SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = 
                new SetParentConfigureAnimotion<Transform, Transform>(cardRect, parentTransform, 3);
            configurables.Add(cardHandSetParentConfigAnimotion);
            //SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion 
            //    = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, GameCreator.Instance.playerOneGraveyard, 4);
            //configurables.Add(blockRayCastConfigAnimotion);
            CombineMotion combineSendToGraveyardMotion = new CombineMotion(this, 1, motionsWaitToGraveyard, configurables);
            return combineSendToGraveyardMotion;
        }

        public Motion OnCardWaitingTarget(Transform cardUI)
        {
            return new SpawnCardTweenMotion(this, cardUI.transform, 1, cardWaitPosition.position, 2);
        }

        public GameObject CreateNewCardPrefab(Card newCard, int PlayerID, CardController cardController)
        {
            // instanciar el prefab de la card y ponerle como parent el canvas
            // ponerlo en el centro de la pantalla
            Vector3 screenCenter = Helper.GetCameraCenterWorldPositionWithZoffset();
            GameObject createdCardGameObject = Instantiate(cardUIPrefab, screenCenter, Quaternion.identity, canvasRootTransform);

            createdCardGameObject.GetComponentInChildren<Text>().text = createdCardGameObject.name;
            MikzeerGame.CardDisplay cardDisplay = createdCardGameObject.GetComponent<MikzeerGame.CardDisplay>();
            createdCardGameObject.name = "CARD N " + newCard.IDInGame + "/" + newCard.CardData.CardName;

            if (cardDisplay != null)
            {
                cardDisplay.Initialized(newCard);
            }
            CardInGameUINEW cardInGameUI = createdCardGameObject.GetComponent<CardInGameUINEW>();
            if (cardInGameUI != null)
            {
                if (PlayerID == 0)
                {
                    cardInGameUI.SetCardData(newCard.CardData, cardController, this, cardHolderPlayerLeft);
                }
                else
                {
                    cardInGameUI.SetCardData(newCard.CardData, cardController, this, cardHolderPlayerRight);
                }
            }
            return createdCardGameObject;
        }

        public void OnCardDescriptionPanelRequired(CardData cardData, Vector2 position)
        {
            infoPanel.SetText(cardData.Description, position, canvasScaler);
        }

        public void OnCardDescriptionPanelClose()
        {
            infoPanel.SetActive(false);
        }
    }     
}