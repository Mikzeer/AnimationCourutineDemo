using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject cardUIPrefab = default;
        [SerializeField] private RectTransform cardHolderP1 = default;
        [SerializeField] private RectTransform cardHolderP2 = default;
        [SerializeField] private RectTransform canvasRootTransform = default;
        [SerializeField] private InfoPanel infoPanel = default; // INFORMACION SOBRE HABILIDADES ESPECIALES DE LAS CARDS 
        [SerializeField] private RectTransform playerOneGraveyardLogo = default;
        [SerializeField] private RectTransform playerOneGraveyard = default;
        [SerializeField] private RectTransform playerTwoGraveyardLogo = default;
        [SerializeField] private RectTransform playerTwoGraveyard = default;
        [SerializeField] private Transform cardWaitPosition = default;

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
                finalCardPosition = cardHolderP1.GetChild(cardHolderP1.childCount - 1).position;
            }
            else
            {
                finalCardPosition = cardHolderP2.GetChild(cardHolderP2.childCount - 1).position;
            }

            Motion motionTweenSpawn = new SpawnCardTweenMotion(this, createdCardGameObject.transform, 1, finalCardPosition, 2);
            motionsSpawn.Add(motionTweenSpawn);

            // tengo que setear el parent
            List<Configurable> configurables = new List<Configurable>();

            SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = null;
            if (PlayerID == 0)
            {
                cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, cardHolderP1, 3);
            }
            else
            {
                cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, cardHolderP2, 3);
            }
            configurables.Add(cardHandSetParentConfigAnimotion);

            //SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = 
            //    new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, GameCreator.Instance.playerOneGraveyard, 4);
            //configurables.Add(blockRayCastConfigAnimotion);

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

        public GameObject CreateNewCardPrefab(Card newCard, int PlayerID, Action<CardInGameUINEW> OnUseAction)
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

            CardInGameUINEW cardInGameUI = createdCardGameObject.AddComponent<CardInGameUINEW>();
            if (cardInGameUI != null)
            {
                if (PlayerID == 0)
                {
                    cardInGameUI.SetPlayerHandTransform(cardHolderP1, OnUseAction, infoPanel.SetText, SetActiveInfoCard);
                }
                else
                {
                    cardInGameUI.SetPlayerHandTransform(cardHolderP2, OnUseAction, infoPanel.SetText, SetActiveInfoCard);
                }

                cardInGameUI.SetRealCardDrop(newCard.OnDropCard, newCard.IDInGame);
            }


            return createdCardGameObject;
        }

        public void SetActiveInfoCard(bool isActive)
        {
            Debug.Log("SE DISPAROR EL IS ACTIVE DESDE LA CARD IS ACTIVE ==" + isActive);
        }
    }     
}