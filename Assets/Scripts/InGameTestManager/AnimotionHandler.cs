using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;

public class AnimotionHandler : MonoBehaviour
{
    #region VARIABLES
    public GameObject shieldPrefab;
    public List<Enemy> enemies;
    public Enemy attackerEnemy;
    public List<AudioClip> audioClips;
    MotionController motionControllerAttack = new MotionController();

    CardManager cardManager = null;
    MotionController motionControllerCardSpawn = new MotionController();
    public GameObject cardUIPrefab;
    public RectTransform canvasRootTransform;
    public RectTransform playersOneHand;
    public Transform cardWaitPosition;
    public RectTransform playerOneGraveyardLogo;
    public RectTransform playerOneGraveyard;
    public AudioSource audioSource;

    #endregion

    #region UPDATE

    private void UpdateControllerSimpleAttack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (motionControllerAttack != null && motionControllerAttack.IsPerforming == false)
            {
                List<PositionerDemo.Motion> motions = new List<PositionerDemo.Motion>();
                PositionerDemo.Motion motionAttack = new AttackMotion(this, attackerEnemy.GetComponent<Animator>(), 1);
                motions.Add(motionAttack);
                PositionerDemo.Motion motionAttackSound = new SoundMotion(this, 1, audioSource, audioClips[0], true);
                motions.Add(motionAttackSound);

                for (int i = 0; i < enemies.Count; i++)
                {
                    PositionerDemo.Motion motionDamage = new DamageMotion(this, enemies[i].animator, 1);
                    motions.Add(motionDamage);
                }

                PositionerDemo.Motion motionDamageSound = new SoundMotion(this, 1, audioSource, audioClips[2], true);
                motions.Add(motionDamageSound);

                CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
                motionControllerAttack.SetUpMotion(combineAttackMotion);

                motionControllerAttack.TryReproduceMotion();
            }
        }
    }

    private void UpdateControllerFiveAttackWithShieldDefense()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (motionControllerAttack != null && motionControllerAttack.IsPerforming == false)
            {
                List<PositionerDemo.Motion> motions = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();

                PositionerDemo.Motion motionAttack = new AttackMotion(this, attackerEnemy.GetComponent<Animator>(), 1);
                motions.Add(motionAttack);
                PositionerDemo.Motion motionAttackSound = new SoundMotion(this, 1, audioSource, audioClips[0], true);
                motions.Add(motionAttackSound);

                for (int i = 0; i < enemies.Count; i++)
                {
                    GameObject shield = Instantiate(shieldPrefab, enemies[i].transform.position, Quaternion.identity);
                    shield.SetActive(true);

                    PositionerDemo.Motion motionDamage = new DamageMotion(this, enemies[i].animator, 1);
                    motions.Add(motionDamage);

                    List<PositionerDemo.Motion> shieldMotions = new List<PositionerDemo.Motion>();

                    PositionerDemo.Motion motionShieldDamage = new ShieldMotion(this, shield.GetComponent<Animator>(), 1);
                    shieldMotions.Add(motionShieldDamage);
                    DestroyGOConfigureAnimotion<Transform, Transform> ShieldDestroyConfigAnimotion = new DestroyGOConfigureAnimotion<Transform, Transform>(shield.transform, 2);
                    configureAnimotion.Add(ShieldDestroyConfigAnimotion);

                    CombineMotion combineShieldMotion = new CombineMotion(this, 1, shieldMotions, configureAnimotion);

                    motions.Add(combineShieldMotion);
                }

                CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);

                motionControllerAttack.SetUpMotion(combineAttackMotion);
                motionControllerAttack.TryReproduceMotion();
            }

        }
    }

    #endregion

    #region CARDS

    public void AddCard()
    {
        //cardManager.AddCard(GetPlayer());

        //if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
        //{
        //    // instanciar el prefab de la card y ponerle como parent el canvas
        //    // ponerlo en el centro de la pantalla
        //    Vector3 screenCenter = Helper.GetCameraCenterWorldPositionWithZoffset();
        //    GameObject createdCardGameObject = Instantiate(cardUIPrefab, screenCenter, Quaternion.identity, canvasRootTransform);

        //    createdCardGameObject.name = "CARD N " + cardIndex;
        //    cardIndex++;
        //    createdCardGameObject.GetComponentInChildren<Text>().text = createdCardGameObject.name;
        //    MikzeerGame.CardUI cardUI = createdCardGameObject.GetComponent<MikzeerGame.CardUI>();


        //    Card newCard = cardManager.TakeCardFromDeck(players[0]);
        //    players[0].PlayersHands.Add(newCard);
        //    MikzeerGame.CardDisplay cardDisplay = createdCardGameObject.GetComponent<MikzeerGame.CardDisplay>();

        //    if (cardDisplay != null)
        //    {
        //        cardDisplay.Initialized(newCard);
        //    }

        //    if (cardUI != null)
        //    {
        //        cardUI.SetPlayerHandTransform(playersOneHand, FireCardUITest, infoPanel.SetText, SetActiveInfoCard);

        //        // generar una action que se active cuando dropeas la carta en la zona DropeableArea
        //        // la Action<CardUI> es la que vamos a disparar desde el AnimotionHandler
        //        // CardUI dropea, si hitea con dropeable es ahi donde disparamos el EVENT
        //    }

        //    Vector3 normalScale = createdCardGameObject.transform.localScale;
        //    Vector3 startScale = new Vector3(0.2f, 0.2f, 1);
        //    createdCardGameObject.transform.localScale = startScale;

        //    // hacerla animacion de instanciamiento a la mano y saliendo un poco de la pantalla

        //    List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
        //    RectTransform cardRect = createdCardGameObject.GetComponent<RectTransform>();
        //    PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, cardRect, 1, normalScale, 1);
        //    motionsSpawn.Add(motionTweenScaleUp);

        //    Vector3 finalCardPosition = playersOneHand.GetChild(playersOneHand.childCount - 1).position;

        //    PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(this, createdCardGameObject.transform, 1, finalCardPosition, 2);
        //    motionsSpawn.Add(motionTweenSpawn);

        //    // tengo que setear el parent
        //    List<PositionerDemo.Configurable> configurables = new List<Configurable>();
        //    SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(createdCardGameObject.transform, playersOneHand, 3);
        //    configurables.Add(cardHandSetParentConfigAnimotion);

        //    CombineMotion combineMoveMotion = new CombineMotion(this, 1, motionsSpawn, configurables);

        //    motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
        //    motionControllerCardSpawn.TryReproduceMotion();
        //}
        //else
        //{
        //    Debug.Log("Is Performing animation");
        //}
    }

    public void FireCardUITest(MikzeerGame.CardUI cardUI)
    {
        Debug.Log("Se disparo el ON CARD USE");

        cardManager.FireCardUITest(cardUI);

        ////OnCardWaitingTarget(cardUI);
        ////OnCardSendToGraveyard(cardUI);

        //bool isSapwn = false;

        //if (motionControllerCardSpawn != null && motionControllerCardSpawn.isPerforming == false)
        //{
        //    List<PositionerDemo.Motion> motionsWaitToGraveyard = new List<PositionerDemo.Motion>();

        //    PositionerDemo.Motion motionTweenToCardWaitPosition = new SpawnCardTweenMotion(this, cardUI.transform, 1, cardWaitPosition.position, 2);
        //    motionsWaitToGraveyard.Add(motionTweenToCardWaitPosition);

        //    PositionerDemo.Motion motionTimer = new TimeMotion(this, 2, 2f);
        //    motionsWaitToGraveyard.Add(motionTimer);

        //    List<PositionerDemo.Configurable> configurables = new List<Configurable>();

        //    if (isSapwn)
        //    {
        //        Vector3 finalCardPosition = playersOneHand.GetChild(playersOneHand.childCount - 1).position;
        //        PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(this, cardUI.transform, 3, finalCardPosition, 2);
        //        motionsWaitToGraveyard.Add(motionTweenSpawn);

        //        // tengo que setear el parent
        //        SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentHandConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, playersOneHand, 4);
        //        configurables.Add(cardHandSetParentHandConfigAnimotion);

        //        SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, playerOneGraveyard, 4);
        //        configurables.Add(blockRayCastConfigAnimotion);
        //    }
        //    else
        //    {
        //        Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);
        //        RectTransform cardRect = cardUI.GetComponent<RectTransform>();
        //        PositionerDemo.Motion motionTweenScaleDownToGraveyard = new ScaleRectTweenMotion(this, cardRect, 3, finalScale, 1);
        //        motionsWaitToGraveyard.Add(motionTweenScaleDownToGraveyard);

        //        Vector3 finalCardPositionGraveyard = playerOneGraveyardLogo.position;
        //        PositionerDemo.Motion motionTweenCardToGraveyard = new SpawnCardTweenMotion(this, cardUI.transform, 3, finalCardPositionGraveyard, 2);
        //        motionsWaitToGraveyard.Add(motionTweenCardToGraveyard);

        //        // tengo que setear el parent
        //        SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentGraveyardConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, playerOneGraveyard, 4);
        //        configurables.Add(cardHandSetParentGraveyardConfigAnimotion);

        //        SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform> blockRayCastConfigAnimotion = new SetCanvasGroupBlockRaycastConfigureAnimotion<MikzeerGame.CardUI, Transform>(cardUI, playerOneGraveyard, 4);
        //        configurables.Add(blockRayCastConfigAnimotion);

        //    }

        //    CombineMotion combineMoveMotion = new CombineMotion(this, 1, motionsWaitToGraveyard, configurables);

        //    motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
        //    motionControllerCardSpawn.TryReproduceMotion();
        //}
    }

    public void SetActiveInfoCard(bool isActive)
    {
        cardManager.SetActiveInfoCard(isActive);

        //if (isActive)
        //{
        //    //Debug.Log("Animacion Card Apareciendo");
        //}
        //else
        //{
        //    //Debug.Log("Animacion Card SE Va");
        //}


        //infoPanel.SetActive(isActive);
    }

    private void OnCardSendToGraveyard(MikzeerGame.CardUI cardUI)
    {
        if (motionControllerCardSpawn != null && motionControllerCardSpawn.IsPerforming == false)
        {
            Vector3 finalScale = new Vector3(0.2f, 0.2f, 1);

            List<PositionerDemo.Motion> motionsWaitToGraveyard = new List<PositionerDemo.Motion>();
            RectTransform cardRect = cardUI.GetComponent<RectTransform>();
            PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, cardRect, 1, finalScale, 1);
            motionsWaitToGraveyard.Add(motionTweenScaleUp);

            Vector3 finalCardPosition = playerOneGraveyardLogo.position;

            PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(this, cardUI.transform, 1, finalCardPosition, 2);
            motionsWaitToGraveyard.Add(motionTweenSpawn);

            // tengo que setear el parent
            List<PositionerDemo.Configurable> configurables = new List<Configurable>();
            SetParentConfigureAnimotion<Transform, Transform> cardHandSetParentConfigAnimotion = new SetParentConfigureAnimotion<Transform, Transform>(cardUI.transform, playerOneGraveyard, 3);
            configurables.Add(cardHandSetParentConfigAnimotion);

            CombineMotion combineMoveMotion = new CombineMotion(this, 1, motionsWaitToGraveyard, configurables);

            motionControllerCardSpawn.SetUpMotion(combineMoveMotion);
            motionControllerCardSpawn.TryReproduceMotion();
        }
    }

    private void OnCardWaitingTarget(MikzeerGame.CardUI cardUI)
    {
        if (motionControllerCardSpawn != null && motionControllerCardSpawn.IsPerforming == false)
        {
            List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();

            Vector3 finalCardPosition = playersOneHand.GetChild(playersOneHand.childCount - 1).position;

            PositionerDemo.Motion motionTweenSpawn = new SpawnCardTweenMotion(this, cardUI.transform, 1, cardWaitPosition.position, 2);

            motionControllerCardSpawn.SetUpMotion(motionTweenSpawn);
            motionControllerCardSpawn.TryReproduceMotion();
        }
    }

    #endregion
}