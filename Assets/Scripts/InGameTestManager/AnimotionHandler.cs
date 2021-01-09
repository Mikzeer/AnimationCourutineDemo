using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;
using UnityEngine.EventSystems;
using System;

public class AnimotionHandler : MonoBehaviour
{
    #region EVENTS

    public static event Action OnChangeTurn;
    public static event Action<int> OnResetActionPoints;

    #endregion

    #region STATIC LAZY SINGLETON

    [SerializeField]
    protected bool dontDestroy;

    private static AnimotionHandler instance;
    public static AnimotionHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AnimotionHandler>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<AnimotionHandler>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as AnimotionHandler;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    #region VARIABLES MOMENTANEAS

    public enum TUTORIALOPTION { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT };
    public TUTORIALOPTION tutorialOption;
    public List<Transform> enemeyTransforms;
    public List<Enemy> enemies;
    public Enemy movingEnemy;
    public Enemy attackerEnemy;

    public List<AudioClip> audioClips;

    MotionController motionControllerAttack = new MotionController();
    MotionController motionControllerSimpleMove = new MotionController();
    MotionController motionControllerCombineMove = new MotionController();
    MotionController motionControllerSpawn = new MotionController();
    MotionController motionControllerCombineSpawn = new MotionController();
    MotionController motionControllerCombineSpawnWithCheck = new MotionController();
    //private int cardIndex = 0;
    private Vector2 startPosition;
    private Vector3 endPostion;
    private Vector3 finishPosition;
    bool va = true;
    private HeatMapGridObject actualHeatMapGridObject;
    Grid<HeatMapGridObject> grid;

    #endregion

    #region VARIABLES

    MotionController motionControllerCreateBoard = new MotionController();
    public Vector3 gridStartPosition = new Vector3(-10, -10, 0);
    public float cellSize = 4f;
    public int withd = 5;
    public int height = 7;
    Board2D board;
    public GameObject tilePrefab;

    private SpawnController spawnCotroller = new SpawnController();
    public GameObject Crane;
    public Transform CraneEnd;
    public GameObject kimbokoPrefab;

    public GameObject shieldPrefab;

    MotionController motionControllerBanner = new MotionController();
    MotionController motionControllerBannerTimmer = new MotionController();
    public RectTransform bannerRect;
    public RectTransform bannerRectTimer;

    public InfoPanel infoPanel;
    public KimbokoInfoPanel kimbokoInfoPanel;

    CardManager cardManager = null;
    MotionController motionControllerCardSpawn = new MotionController();
    public GameObject cardUIPrefab;
    public RectTransform canvasRootTransform;
    public RectTransform playersOneHand;
    public Transform cardWaitPosition;
    public RectTransform playerOneGraveyardLogo;
    public RectTransform playerOneGraveyard;

    Camera cam;
    public AudioSource audioSource;
    UnitMovePositioner movePositioner;
    private GameObject[,] tiles;
    Tile actualTileObject;
    Player[] players;

    #endregion

    void Start()
    {
        cam = Camera.main;

        //SetBoard();

        CreatePlayers();
        CreateDeck();
        CreateNewBoard();
        switch (tutorialOption)
        {
            case TUTORIALOPTION.TWO:
                StartMotionControllerTwo();
                break;
            case TUTORIALOPTION.FOUR:
                StartMotionControllerFour();
                break;
            default:
                break;
        }

        SpawnAbility.OnActionEndExecute += SpawnInfoTest;
    }

    #region PLAYER

    Player actualPlayerTurn;

    public void SetPlayerTurn(Player player)
    {
        actualPlayerTurn = player;
    }

    public Player GetPlayer()
    {
        return actualPlayerTurn;
    }

    public void ChangeTurn()
    {
        for (int i = 0; i < actualPlayerTurn.Abilities.Count; i++)
        {
            ABILITYTYPE abType = (ABILITYTYPE)i;
            if (actualPlayerTurn.Abilities[abType].actionStatus == ABILITYEXECUTIONSTATUS.STARTED)
            {
                Debug.Log("Waiting for action to end");
                return;
            }
        }

        // TAMBIEN DEBERIAMOS RECORRER LA LISTA DE UNIDADES Y VERIFICAR SI NO TENEMOS NINGUNA ACCION EFECTUANDOSE

        if (actualPlayerTurn == players[0])
        {
            actualPlayerTurn = players[1];
        }
        else
        {
            actualPlayerTurn = players[0];
        }

        OnChangeTurn?.Invoke();
        OnResetActionPoints?.Invoke(actualPlayerTurn.PlayerID);
    }

    #endregion

    #region SETGAME

    private void SetBoard()
    {
        grid = new Grid<HeatMapGridObject>(withd, height, cellSize, gridStartPosition, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        movePositioner = new UnitMovePositioner(cellSize);

        tiles = new GameObject[withd, height];

        List<PositionerDemo.Motion> motionsCreateBoard = new List<PositionerDemo.Motion>();

        int index = 1;
        GameObject tileParent = new GameObject("TileParent");

        for (int x = 0; x < withd; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 thisTileFinalPosition = grid.GetGridObject(x, y).GetRealWorldLocation();

                tiles[x, y] = Instantiate(tilePrefab);
                tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                tiles[x, y].transform.localScale *= cellSize;
                tiles[x, y].transform.SetParent(tileParent.transform);

                // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                motionsCreateBoard.Add(motionTweenMove);
            }
            index++;
        }

        CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCreateBoard);

        List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
        List<Configurable> configureAnimotion = new List<Configurable>();

        Vector3 normalScale = bannerRect.localScale;
        Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

        // SE ACTIVA
        SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 1);
        configureAnimotion.Add(KimbokoActiveConfigAnimotion);

        // REPRODUCE LA TWEEN
        PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, bannerRect, 2, finalScale, 2);
        motionsSpawn.Add(motionTweenScaleUp);
        PositionerDemo.Motion motionTweenScaleDown = new ScaleRectTweenMotion(this, bannerRect, 3, normalScale, 2);
        motionsSpawn.Add(motionTweenScaleDown);

        // SE DESACTIVA
        SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 4, true, false);
        configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);

        CombineMotion combinSecondMoveMotion = new CombineMotion(this, 2, motionsSpawn, configureAnimotion);

        List<PositionerDemo.Motion> motionsFinalCreateBoard = new List<PositionerDemo.Motion>();
        motionsFinalCreateBoard.Add(combinMoveMotion);
        motionsFinalCreateBoard.Add(combinSecondMoveMotion);

        CombineMotion combinFinalMotion = new CombineMotion(this, 1, motionsFinalCreateBoard);

        motionControllerCreateBoard.SetUpMotion(combinFinalMotion);
        motionControllerCreateBoard.TryReproduceMotion();
    }

    public Board2D GetBoard()
    {
        return board;
    }

    private void CreatePlayers()
    {
        players = new Player[2];

        Stack<Card> deckPlayerOne = new Stack<Card>();
        Stack<Card> deckPlayerTwo = new Stack<Card>();

        //players[0] = new Player(0, PLAYERTYPE.PLAYER, deckPlayerOne);
        //players[1] = new Player(1, PLAYERTYPE.PLAYER, deckPlayerTwo);

        players[0] = new Player(0);
        players[1] = new Player(1);


        SetPlayerTurn(players[0]);
    }

    private void CreateDeck()
    {
        //cardManager.CreateDeck(players[0], playerOneCards);
        //cardManager.CreateDeck(players[1], playerTwoCards);
    }

    private void CreateNewBoard()
    {
        //players = new Player[2];

        //Stack<Card> deckPlayerOne = new Stack<Card>();
        //Stack<Card> deckPlayerTwo = new Stack<Card>();

        //players[0] = new Player(0, PLAYERTYPE.PLAYER, deckPlayerOne);
        //players[1] = new Player(1, PLAYERTYPE.PLAYER, deckPlayerTwo);

        //SetPlayerTurn(players[0]);

        //cardManager.CreateDeck(players[0], playerOneCards);
        //cardManager.CreateDeck(players[1], playerTwoCards);

        board = new Board2D(height, withd, players, gridStartPosition);

        movePositioner = new UnitMovePositioner(board.GetTileSize());
        tiles = new GameObject[withd + 4, height];

        withd += 4;

        int index = 1;
        GameObject tileParent = new GameObject("TileParent");

        List<PositionerDemo.Motion> motionsCreateBoard = new List<PositionerDemo.Motion>();

        for (int x = 0; x < withd; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == 1)
                {
                    tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.SetParent(tileParent.transform);
                    continue;
                }
                if (x == 9 || x == 10)
                {
                    tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.SetParent(tileParent.transform);
                    continue;
                }

                Vector3 thisTileFinalPosition = board.GetGridObject(x, y).GetRealWorldLocation();

                tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                tiles[x, y].transform.SetParent(tileParent.transform);

                // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                motionsCreateBoard.Add(motionTweenMove);
            }
            index++;
        }

        // para las spawn tiles
        Vector2 yOffset = new Vector2(0, 10);

        Vector3 pOneNexusFinalPosition = board.GetPlayerNexusWorldPosition(players[0]);
        tiles[0, 0].transform.position = new Vector3(pOneNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
        PositionerDemo.Motion motionTweenNexusP1Move = new MoveTweenMotion(this, tiles[0, 0].transform, index, pOneNexusFinalPosition, 1);
        motionsCreateBoard.Add(motionTweenNexusP1Move);

        Vector3 pTwoNexusFinalPosition = board.GetPlayerNexusWorldPosition(players[1]);
        tiles[9, 0].transform.position = new Vector3(pTwoNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
        PositionerDemo.Motion motionTweenNexusP2Move = new MoveTweenMotion(this, tiles[9, 0].transform, index, pTwoNexusFinalPosition, 1);
        motionsCreateBoard.Add(motionTweenNexusP2Move);

        CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCreateBoard);

        ////List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
        ////List<Configurable> configureAnimotion = new List<Configurable>();

        ////Vector3 normalScale = bannerRect.localScale;
        ////Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

        ////// SE ACTIVA
        ////SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 1);
        ////configureAnimotion.Add(KimbokoActiveConfigAnimotion);

        ////// REPRODUCE LA TWEEN
        ////PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, bannerRect, 2, finalScale, 2);
        ////motionsSpawn.Add(motionTweenScaleUp);
        ////PositionerDemo.Motion motionTweenScaleDown = new ScaleRectTweenMotion(this, bannerRect, 3, normalScale, 2);
        ////motionsSpawn.Add(motionTweenScaleDown);

        ////// SE DESACTIVA
        ////SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 4, true, false);
        ////configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);

        ////CombineMotion combinSecondMoveMotion = new CombineMotion(this, 2, motionsSpawn, configureAnimotion);

        //List<PositionerDemo.Motion> motionsFinalCreateBoard = new List<PositionerDemo.Motion>();
        //motionsFinalCreateBoard.Add(combinMoveMotion);
        ////motionsFinalCreateBoard.Add(combinSecondMoveMotion);

        //CombineMotion combinFinalMotion = new CombineMotion(this, 1, motionsFinalCreateBoard);

        motionControllerCreateBoard.SetUpMotion(combinMoveMotion);
        motionControllerCreateBoard.TryReproduceMotion();
    }

    private void StartMotionControllerTwo()
    {
        Vector3[] finalPositions = movePositioner.GetPositions(grid.GetGridObject(0, 0).GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));

        // Necesitamos los transform a posicionarse
        if (enemeyTransforms != null && enemeyTransforms.Count > 0)
        {
            enemies = new List<Enemy>();
            for (int i = 0; i < enemeyTransforms.Count; i++)
            {
                //enemeyTransforms[i].position = grid.GetGridObject(0,0).GetRealWorldLocation();
                enemeyTransforms[i].position = finalPositions[i];
                enemies.Add(enemeyTransforms[i].GetComponent<Enemy>());
            }
        }
    }

    private void StartMotionControllerFour()
    {
        movePositioner = new UnitMovePositioner(cellSize);
        Vector3[] finalPositions = movePositioner.GetPositions(grid.GetGridObject(0, 0).GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));

        // Necesitamos los transform a posicionarse
        if (enemeyTransforms != null && enemeyTransforms.Count > 0)
        {
            enemies = new List<Enemy>();
            for (int i = 0; i < enemeyTransforms.Count; i++)
            {
                //enemeyTransforms[i].position = grid.GetGridObject(0,0).GetRealWorldLocation();
                enemeyTransforms[i].position = finalPositions[i];
                enemies.Add(enemeyTransforms[i].GetComponent<Enemy>());
            }
        }
    }

    #endregion

    #region UPDATE

    void Update()
    {
        NineTest();
        /*
        switch (tutorialOption)
        {
            case TUTORIALOPTION.ONE:
                UpdateControllerOne();
                break;
            case TUTORIALOPTION.TWO:
                UpdateControllerTwo();
                break;
            case TUTORIALOPTION.THREE:
                UpdateControllerThree();
                break;
            case TUTORIALOPTION.FOUR:
                UpdateControllerFour();
                break;
            case TUTORIALOPTION.FIVE:
                UpdateControllerFive();
                break;
            case TUTORIALOPTION.SIX:
                UpdateControllerSix();
                break;
            case TUTORIALOPTION.SEVEN:
                UpdateControllerSeven();
                break;
            //case TUTORIALOPTION.EIGHT:
            //    UpdateControllerEight();
            //    break;
            default:
                break;
        }

        UpdateControllerEight();
        */
    }

    private void UpdateControllerOne()
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

            if (motionControllerSimpleMove != null && motionControllerSimpleMove.IsPerforming == false)
            {
                startPosition = movingEnemy.transform.position;

                Vector2 oldPosition = startPosition; // 5.96,-3.15
                if (va)
                {
                    endPostion = startPosition + new Vector2(0, 8); // 5.96,11.85
                    va = false;
                }
                else
                {
                    endPostion = startPosition + new Vector2(0, -8); // 5.96,11.85
                    va = true;
                }
                finishPosition = endPostion;

                List<PositionerDemo.Motion> motionsMove = new List<PositionerDemo.Motion>();
                PositionerDemo.Motion motionMove = new MoveMotion(this, movingEnemy.GetComponent<Animator>(), 1);
                motionsMove.Add(motionMove);

                PositionerDemo.Motion motionMoveSound = new SoundMotion(this, 1, audioSource, audioClips[4], false, true);
                motionsMove.Add(motionMoveSound);



                List<PositionerDemo.Motion> motionsStopMove = new List<PositionerDemo.Motion>();
                List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, movingEnemy.transform, 1, endPostion);
                PositionerDemo.Motion motionIdlle = new IdlleMotion(this, movingEnemy.GetComponent<Animator>(), 2, true);
                motionsStopMove.Add(motionTweenMove);
                motionsStopMove.Add(motionIdlle);

                AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(audioSource);
                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                configurables.Add(stopSoundConfigureAnimotion);

                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsStopMove, configurables);
                motionsMove.Add(combineStopMotion);

                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsMove);

                motionControllerSimpleMove.SetUpMotion(combinMoveMotion);
                motionControllerSimpleMove.TryReproduceMotion();

                startPosition = endPostion;
                endPostion = oldPosition;
            }
        }
    }

    private void UpdateControllerTwo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (heatMapGridObject != null)
            {
                if (motionControllerCombineMove != null && motionControllerCombineMove.IsPerforming == false)
                {
                    Vector3 actualPosition = grid.GetGridObject(enemeyTransforms[0].position).GetRealWorldLocation();
                    Dictionary<Enemy, Vector3[]> enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies.ToArray(), movePositioner.GetPositionType(enemies.Count), heatMapGridObject.GetRealWorldLocation(), actualPosition);
                    List<PositionerDemo.Motion> motionsCombineMove = new List<PositionerDemo.Motion>();

                    PositionerDemo.Motion motionMoveSound = new SoundMotion(this, 1, audioSource, audioClips[4], false, true);
                    motionsCombineMove.Add(motionMoveSound);

                    int index = 0;
                    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
                    {
                        AnimatedMotion motionMove = new MoveMotion(this, entry.Key.GetComponent<Animator>(), 1);
                        motionsCombineMove.Add(motionMove);

                        List<PositionerDemo.Motion> extraMotionsCombineStopMove = new List<PositionerDemo.Motion>();
                        PositionerDemo.Motion motionTweenMove = new MovePathTweenMotion(this, entry.Key.transform, 1, entry.Value);
                        PositionerDemo.Motion motionIdlle = new IdlleMotion(this, entry.Key.GetComponent<Animator>(), 2, true);
                        extraMotionsCombineStopMove.Add(motionTweenMove);
                        extraMotionsCombineStopMove.Add(motionIdlle);

                        // esto solo lo hago para detener el sonido de los pasos
                        if (enmiesAndPathToMove.Count - 1 == index)
                        {
                            List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                            AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(audioSource);
                            StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                            configurables.Add(stopSoundConfigureAnimotion);
                            CombineMotion extraCombineMoveStopMotion = new CombineMotion(this, 1, extraMotionsCombineStopMove, configurables);
                            motionsCombineMove.Add(extraCombineMoveStopMotion);
                        }
                        else
                        {
                            CombineMotion extraCombineMoveStopMotion = new CombineMotion(this, 1, extraMotionsCombineStopMove);
                            motionsCombineMove.Add(extraCombineMoveStopMotion);
                        }

                        index++;
                    }

                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCombineMove);
                    motionControllerCombineMove.SetUpMotion(combinMoveMotion);
                    motionControllerCombineMove.TryReproduceMotion();
                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
    }

    private void UpdateControllerThree()
    {
        // Necesitamos que los Transform reconozcan un input
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (heatMapGridObject != null)
            {
                if (motionControllerSpawn != null && motionControllerSpawn.IsPerforming == false)
                {
                    List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                    List<Configurable> configureAnimotion = new List<Configurable>();

                    // TWEENCRANE / SPAWNCRANE / TWEEN KIMBOKO / TWEENBACKCRANE
                    // KIMBOKO POSITION END CRANE / KIMBOKO SET ACTIVE TRUE / KIMBOKO IDLEE / CRANE IDLLE

                    int craneTweenSpeedVelocity = 10;
                    int kimbokoTweenSpeedVelocity = 10;

                    // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
                    Vector3 craneStartPosition;
                    Vector3 craneEndPostion;
                    //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
                    craneStartPosition = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Crane.transform.position.y, 0);
                    Crane.transform.position = craneStartPosition;
                    craneEndPostion = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Helper.GetCameraTopBorderYWorldPostion().y);

                    //A CRANE//GRUA SET ACTIVE = TRUE // INSTANCIAMOS KIMBOKO SET ACTIVE FALSE
                    Crane.SetActive(true);
                    GameObject goKimbok = Instantiate(kimbokoPrefab);
                    goKimbok.transform.position = CraneEnd.position;
                    // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
                    goKimbok.SetActive(true);

                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, Crane.transform, 1, craneEndPostion, craneTweenSpeedVelocity);
                    motionsSpawn.Add(motionTweenMove);

                    ////C ANIMATION CRANESPAWNING
                    PositionerDemo.Motion motionCraneSpawn = new SpawnMotion(this, Crane.GetComponent<Animator>(), 2);
                    motionsSpawn.Add(motionCraneSpawn);

                    PositionerDemo.Motion motionSpawnSound = new SoundMotion(this, 2, audioSource, audioClips[3], false);
                    motionsSpawn.Add(motionSpawnSound);


                    KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, CraneEnd, 3);
                    configureAnimotion.Add(KimbokoPositionConfigAnimotion);
                    SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(goKimbok.transform, 3);
                    configureAnimotion.Add(KimbokoActiveConfigAnimotion);

                    ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
                    PositionerDemo.Motion motionKimbokoTweenMove = new MoveTweenMotion(this, goKimbok.transform, 4, heatMapGridObject.GetRealWorldLocation(), kimbokoTweenSpeedVelocity);
                    motionsSpawn.Add(motionKimbokoTweenMove);
                    //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
                    PositionerDemo.Motion motionTweenBackCraneMove = new MoveTweenMotion(this, Crane.transform, 5, craneStartPosition, craneTweenSpeedVelocity);
                    motionsSpawn.Add(motionTweenBackCraneMove);

                    // FINISH //
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 6);
                    configureAnimotion.Add(kimbokoIdlleConfigureAnimotion);
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> craneIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(Crane.GetComponent<Animator>(), 6);
                    configureAnimotion.Add(craneIdlleConfigureAnimotion);

                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);

                    // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
                    goKimbok.SetActive(false);

                    motionControllerSpawn.SetUpMotion(combinMoveMotion);
                    motionControllerSpawn.TryReproduceMotion();
                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
    }

    private void UpdateControllerFour()
    {
        // Necesitamos que los Transform reconozcan un input
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (heatMapGridObject != null)
            {
                if (heatMapGridObject.CanIAddEnemies() == false)
                {
                    Debug.Log("FULL OF ENEMIES");
                    return;
                }
                if (motionControllerCombineSpawn != null && motionControllerCombineSpawn.IsPerforming == false)
                {
                    List<PositionerDemo.Motion> motionsSpawnCombine = new List<PositionerDemo.Motion>();
                    List<Configurable> configureAnimotion = new List<Configurable>();

                    // ACA TENEMOS QUE VER SI HAY MAS DE UNA UNIDAD EN LA TILE
                    // MIENTRAS BAJA EL CRANE // UNIDADES EN LA TILE SE REPOSICIONAN A LOS COSTADOS

                    // ACA TENGO LA POSICION DE LA TILA DONDE DEBERIAN ESTAR LAS UNIDADES
                    Vector3 actualPosition = heatMapGridObject.GetRealWorldLocation();
                    // AHORA TENGO QUE GENERAR EL CUADRADO DE POSICIONES PARA CADA UNA
                    Vector3[] squarePositions = movePositioner.GetPositions(actualPosition, POSITIONTYPE.SQUARE);

                    PositionerDemo.Motion motionMoveSound = new SoundMotion(this, 1, audioSource, audioClips[4], false, true);
                    motionsSpawnCombine.Add(motionMoveSound);

                    // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        List<PositionerDemo.Motion> motionsCombineSpawnMoveSquare = new List<PositionerDemo.Motion>();
                        PositionerDemo.Motion motionMove = new MoveMotion(this, enemies[i].animator, 1);
                        motionsCombineSpawnMoveSquare.Add(motionMove);

                        List<PositionerDemo.Motion> motionsCombineSpawnStopMoveSquare = new List<PositionerDemo.Motion>();
                        PositionerDemo.Motion motionTwMove = new MoveTweenMotion(this, enemies[i].transform, 1, squarePositions[i]);
                        PositionerDemo.Motion motionIdlle = new IdlleMotion(this, enemies[i].animator, 2, true);
                        motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                        motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                        //CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        //motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                        //CombineMotion combinSquarePositionMotion = new CombineMotion(this, 1, motionsCombineSpawnMoveSquare);
                        //motionsSpawnCombine.Add(combinSquarePositionMotion);


                        // esto solo lo hago para detener el sonido de los pasos
                        if (enemies.Count - 1 == i)
                        {
                            List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                            AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(audioSource);
                            StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                            configurables.Add(stopSoundConfigureAnimotion);

                            CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                            motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                        }
                        else
                        {
                            CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                            motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                        }

                        CombineMotion combinSquarePositionMotion = new CombineMotion(this, 1, motionsCombineSpawnMoveSquare);
                        motionsSpawnCombine.Add(combinSquarePositionMotion);

                    }



                    // TWEENCRANE / SPAWNCRANE / TWEEN KIMBOKO / TWEENBACKCRANE
                    // KIMBOKO POSITION END CRANE / KIMBOKO SET ACTIVE TRUE / KIMBOKO IDLEE / CRANE IDLLE

                    int craneTweenSpeedVelocity = 10;
                    int kimbokoTweenSpeedVelocity = 10;

                    // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
                    Vector3 craneStartPosition;
                    Vector3 craneEndPostion;
                    //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
                    craneStartPosition = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Crane.transform.position.y, 0);
                    Crane.transform.position = craneStartPosition;
                    craneEndPostion = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Helper.GetCameraTopBorderYWorldPostion().y);

                    //A CRANE//GRUA SET ACTIVE = TRUE // INSTANCIAMOS KIMBOKO SET ACTIVE FALSE
                    Crane.SetActive(true);
                    GameObject goKimbok = Instantiate(kimbokoPrefab);
                    goKimbok.transform.position = CraneEnd.position;
                    // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
                    goKimbok.SetActive(true);

                    Enemy enemy = goKimbok.GetComponent<Enemy>();
                    heatMapGridObject.AddEnemy(enemy);


                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, Crane.transform, 1, craneEndPostion, craneTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionTweenMove);

                    ////C ANIMATION CRANESPAWNING
                    PositionerDemo.Motion motionCraneSpawn = new SpawnMotion(this, Crane.GetComponent<Animator>(), 2);
                    motionsSpawnCombine.Add(motionCraneSpawn);

                    PositionerDemo.Motion motionSpawnSound = new SoundMotion(this, 2, audioSource, audioClips[3], false);
                    motionsSpawnCombine.Add(motionSpawnSound);

                    KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, CraneEnd, 3);
                    configureAnimotion.Add(KimbokoPositionConfigAnimotion);
                    SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(goKimbok.transform, 3);
                    configureAnimotion.Add(KimbokoActiveConfigAnimotion);

                    ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
                    PositionerDemo.Motion motionKimbokoTweenMove = new MoveTweenMotion(this, goKimbok.transform, 4, heatMapGridObject.GetRealWorldLocation(), kimbokoTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionKimbokoTweenMove);
                    //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
                    PositionerDemo.Motion motionTweenBackCraneMove = new MoveTweenMotion(this, Crane.transform, 5, craneStartPosition, craneTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionTweenBackCraneMove);

                    // FINISH //
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 6);
                    configureAnimotion.Add(kimbokoIdlleConfigureAnimotion);
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> craneIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(Crane.GetComponent<Animator>(), 6);
                    configureAnimotion.Add(craneIdlleConfigureAnimotion);

                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawnCombine, configureAnimotion);

                    // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
                    goKimbok.SetActive(false);

                    motionControllerCombineSpawn.SetUpMotion(combinMoveMotion);
                    motionControllerCombineSpawn.TryReproduceMotion();
                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
    }

    private void UpdateControllerFive()
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

    private void UpdateControllerSix()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (motionControllerBanner != null && motionControllerBanner.IsPerforming == false)
            {
                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();

                Vector3 normalScale = bannerRect.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

                // SE ACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 1);
                configureAnimotion.Add(KimbokoActiveConfigAnimotion);

                // REPRODUCE LA TWEEN
                PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, bannerRect, 2, finalScale);
                motionsSpawn.Add(motionTweenScaleUp);
                PositionerDemo.Motion motionTweenScaleDown = new ScaleRectTweenMotion(this, bannerRect, 3, normalScale);
                motionsSpawn.Add(motionTweenScaleDown);

                // SE DESACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 4, true, false);
                configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);

                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);

                motionControllerBanner.SetUpMotion(combinMoveMotion);
                motionControllerBanner.TryReproduceMotion();
            }
            else
            {
                Debug.Log("Is Performing animation");
            }

            if (motionControllerBannerTimmer != null && motionControllerBannerTimmer.IsPerforming == false)
            {
                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();

                Vector3 normalScale = bannerRectTimer.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

                // SE ACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRectTimer, 1);
                configureAnimotion.Add(KimbokoActiveConfigAnimotion);

                // REPRODUCE LA TWEEN
                PositionerDemo.Motion motionTimer = new TimeMotion(this, 2, 15f);
                motionsSpawn.Add(motionTimer);

                // SE DESACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRectTimer, 4, true, false);
                configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);

                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);

                motionControllerBannerTimmer.SetUpMotion(combinMoveMotion);
                motionControllerBannerTimmer.TryReproduceMotion();
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }
    }

    private void UpdateControllerSeven()
    {
        // Necesitamos que los Transform reconozcan un input
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (heatMapGridObject != null)
            {
                // 1 - PRIMERO ME FIJO SI PUEDO AGREGAR ENEMIGOS, SI NO PUEDO, DIRECTAMENTE VOY A ABORTAR
                if (heatMapGridObject.CanIAddEnemies() == false)
                {
                    Debug.Log("FULL OF ENEMIES");
                    return;
                }

                if (motionControllerCombineSpawnWithCheck != null && motionControllerCombineSpawnWithCheck.IsPerforming == false)
                {
                    List<PositionerDemo.Motion> motionsSpawnCombine = new List<PositionerDemo.Motion>();
                    List<Configurable> configureAnimotion = new List<Configurable>();

                    // 2 - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
                    if (heatMapGridObject.GetEnemies().Count > 0)
                    {
                        List<Enemy> kimbokoToRepositionSquare = heatMapGridObject.GetEnemies();

                        // ACA TENEMOS QUE VER SI HAY MAS DE UNA UNIDAD EN LA TILE
                        // MIENTRAS BAJA EL CRANE // UNIDADES EN LA TILE SE REPOSICIONAN A LOS COSTADOS

                        // ACA TENGO LA POSICION DE LA TILA DONDE DEBERIAN ESTAR LAS UNIDADES
                        Vector3 actualPosition = heatMapGridObject.GetRealWorldLocation();
                        // AHORA TENGO QUE GENERAR EL CUADRADO DE POSICIONES PARA CADA UNA
                        Vector3[] squarePositions = movePositioner.GetPositions(actualPosition, POSITIONTYPE.SQUARE);

                        PositionerDemo.Motion motionMoveSound = new SoundMotion(this, 1, audioSource, audioClips[4], false, true);
                        motionsSpawnCombine.Add(motionMoveSound);

                        // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                        for (int i = 0; i < kimbokoToRepositionSquare.Count; i++)
                        {
                            List<PositionerDemo.Motion> motionsCombineSpawnMoveSquare = new List<PositionerDemo.Motion>();
                            PositionerDemo.Motion motionMove = new MoveMotion(this, kimbokoToRepositionSquare[i].animator, 1);
                            motionsCombineSpawnMoveSquare.Add(motionMove);

                            List<PositionerDemo.Motion> motionsCombineSpawnStopMoveSquare = new List<PositionerDemo.Motion>();
                            PositionerDemo.Motion motionTwMove = new MoveTweenMotion(this, kimbokoToRepositionSquare[i].transform, 1, squarePositions[i], 1);
                            PositionerDemo.Motion motionIdlle = new IdlleMotion(this, kimbokoToRepositionSquare[i].animator, 2, true);
                            motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                            motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                            // esto solo lo hago para detener el sonido de los pasos
                            if (kimbokoToRepositionSquare.Count - 1 == i)
                            {
                                List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                                AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(audioSource);
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                                configurables.Add(stopSoundConfigureAnimotion);

                                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                                motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                            }
                            else
                            {
                                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                                motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                            }


                            //CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                            //motionsCombineSpawnMoveSquare.Add(combineStopMotion);

                            CombineMotion combinSquarePositionMotion = new CombineMotion(this, 1, motionsCombineSpawnMoveSquare);

                            motionsSpawnCombine.Add(combinSquarePositionMotion);
                        }
                    }

                    // TWEENCRANE / SPAWNCRANE / TWEEN KIMBOKO / TWEENBACKCRANE
                    // KIMBOKO POSITION END CRANE / KIMBOKO SET ACTIVE TRUE / KIMBOKO IDLEE / CRANE IDLLE
                    int craneTweenSpeedVelocity = 1;
                    int kimbokoTweenSpeedVelocity = 1;

                    // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
                    Vector3 craneStartPosition;
                    Vector3 craneEndPostion;
                    //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
                    craneStartPosition = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Crane.transform.position.y, 0);
                    Crane.transform.position = craneStartPosition;
                    craneEndPostion = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Helper.GetCameraTopBorderYWorldPostion().y);

                    //A CRANE//GRUA SET ACTIVE = TRUE // INSTANCIAMOS KIMBOKO SET ACTIVE FALSE
                    Crane.SetActive(true);
                    GameObject goKimbok = Instantiate(kimbokoPrefab);
                    goKimbok.transform.position = CraneEnd.position;
                    // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
                    goKimbok.SetActive(true);

                    Enemy enemy = goKimbok.GetComponent<Enemy>();
                    heatMapGridObject.AddEnemy(enemy);

                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, Crane.transform, 1, craneEndPostion, craneTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionTweenMove);

                    ////C ANIMATION CRANESPAWNING
                    PositionerDemo.Motion motionCraneSpawn = new SpawnMotion(this, Crane.GetComponent<Animator>(), 2);
                    motionsSpawnCombine.Add(motionCraneSpawn);

                    PositionerDemo.Motion motionSpawnSound = new SoundMotion(this, 2, audioSource, audioClips[3], false);
                    motionsSpawnCombine.Add(motionSpawnSound);

                    KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, CraneEnd, 3);
                    configureAnimotion.Add(KimbokoPositionConfigAnimotion);
                    SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(goKimbok.transform, 3);
                    configureAnimotion.Add(KimbokoActiveConfigAnimotion);

                    ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
                    PositionerDemo.Motion motionKimbokoTweenMove = new MoveTweenMotion(this, goKimbok.transform, 4, heatMapGridObject.GetRealWorldLocation(), kimbokoTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionKimbokoTweenMove);

                    KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleFirstConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 5);
                    configureAnimotion.Add(kimbokoIdlleFirstConfigureAnimotion);

                    //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
                    PositionerDemo.Motion motionTweenBackCraneMove = new MoveTweenMotion(this, Crane.transform, 6, craneStartPosition, craneTweenSpeedVelocity);
                    motionsSpawnCombine.Add(motionTweenBackCraneMove);

                    //  - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
                    if (heatMapGridObject.GetEnemies().Count > 1)
                    {
                        List<Enemy> kimbokoToRepositionSquare = heatMapGridObject.GetEnemies();
                        Vector3 actualPosition = heatMapGridObject.GetRealWorldLocation();
                        POSITIONTYPE positionTypeToRearrange = movePositioner.GetPositionType(kimbokoToRepositionSquare.Count);
                        Vector3[] finalRearrangePositions = movePositioner.GetPositions(actualPosition, positionTypeToRearrange);

                        PositionerDemo.Motion motionMoveSound = new SoundMotion(this, 6, audioSource, audioClips[4], false, true);
                        motionsSpawnCombine.Add(motionMoveSound);

                        // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                        for (int i = 0; i < kimbokoToRepositionSquare.Count; i++)
                        {
                            List<PositionerDemo.Motion> motionsCombineSpawnMoveSquare = new List<PositionerDemo.Motion>();

                            int shortNameHash = Animator.StringToHash("Base Layer" + ".Idlle");
                            //Debug.Log("shortNameHash " + shortNameHash);

                            PositionerDemo.Motion motionMove = new MoveMotion(this, kimbokoToRepositionSquare[i].animator, 1, false, shortNameHash);
                            motionsCombineSpawnMoveSquare.Add(motionMove);

                            List<PositionerDemo.Motion> motionsCombineSpawnStopMoveSquare = new List<PositionerDemo.Motion>();

                            PositionerDemo.Motion motionTwMove = new MoveTweenMotion(this, kimbokoToRepositionSquare[i].transform, 1, finalRearrangePositions[i], 1);
                            motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                            PositionerDemo.Motion motionIdlle = new IdlleMotion(this, kimbokoToRepositionSquare[i].animator, 2, true);
                            motionsCombineSpawnStopMoveSquare.Add(motionIdlle);


                            // esto solo lo hago para detener el sonido de los pasos
                            if (kimbokoToRepositionSquare.Count - 1 == i)
                            {
                                List<PositionerDemo.Configurable> configurables = new List<Configurable>();
                                AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(audioSource);
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                                configurables.Add(stopSoundConfigureAnimotion);

                                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                                motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                            }
                            else
                            {
                                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                                motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                            }

                            //CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                            //motionsCombineSpawnMoveSquare.Add(combineStopMotion);

                            CombineMotion combinSquarePositionMotion = new CombineMotion(this, 6, motionsCombineSpawnMoveSquare);

                            motionsSpawnCombine.Add(combinSquarePositionMotion);
                        }
                    }

                    // FINISH //
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 7);
                    configureAnimotion.Add(kimbokoIdlleConfigureAnimotion);
                    KimbokoIdlleConfigureAnimotion<Animator, Transform> craneIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(Crane.GetComponent<Animator>(), 7);
                    configureAnimotion.Add(craneIdlleConfigureAnimotion);

                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawnCombine, configureAnimotion);

                    // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
                    goKimbok.SetActive(false);

                    motionControllerCombineSpawnWithCheck.SetUpMotion(combinMoveMotion);
                    motionControllerCombineSpawnWithCheck.TryReproduceMotion();

                    // update del info de la tile
                    UpdateKimbokoInfoPanel(heatMapGridObject);

                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
    }

    private void UpdateControllerEight()
    {
        if (IsMouseOverUIWithIgnores())
        {
            return;
        }

        HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));

        if (heatMapGridObject != null)
        {
            if (actualHeatMapGridObject != null)
            {
                if (actualHeatMapGridObject == heatMapGridObject)
                {
                    //UpdateUnitInfoPanel(actualHeatMapGridObject);
                    return;
                }
                else if (actualHeatMapGridObject != heatMapGridObject)
                {
                    tiles[actualHeatMapGridObject.x, actualHeatMapGridObject.y].GetComponent<SpriteRenderer>().color = Color.green;
                    actualHeatMapGridObject = heatMapGridObject;
                    tiles[actualHeatMapGridObject.x, actualHeatMapGridObject.y].GetComponent<SpriteRenderer>().color = Color.blue;
                    UpdateKimbokoInfoPanel(actualHeatMapGridObject);
                }
            }
            else
            {
                actualHeatMapGridObject = heatMapGridObject;
                tiles[actualHeatMapGridObject.x, actualHeatMapGridObject.y].GetComponent<SpriteRenderer>().color = Color.blue;
                UpdateKimbokoInfoPanel(actualHeatMapGridObject);
            }
        }
        else
        {
            if (actualHeatMapGridObject != null)
            {
                tiles[actualHeatMapGridObject.x, actualHeatMapGridObject.y].GetComponent<SpriteRenderer>().color = Color.green;
                actualHeatMapGridObject = null;
                kimbokoInfoPanel.SetText(false);
            }
        }
    }

    private void NineTest()
    {
        if (IsMouseOverUIWithIgnores())
        {
            return;
        }

        Tile TileObject = board.GetGridObject(Helper.GetMouseWorldPosition(cam));

        if (Input.GetMouseButtonDown(0))
        {
            if (TileObject != null)
            {
                if (actualPlayerTurn.Abilities[ABILITYTYPE.SPAWN].OnTryEnter() == true)
                {
                    spawnCotroller.OnTrySpawn(TileObject, actualPlayerTurn);
                }
                else
                {
                    Debug.Log("CANT ENTER");
                }
            }
        }

        if (TileObject != null)
        {
            if (actualTileObject != null)
            {
                if (actualTileObject == TileObject)
                {
                    //UpdateKimbokoTileInfoPanel(actualTileObject);
                    return;
                }
                else if (actualTileObject != TileObject)
                {
                    tiles[actualTileObject.PosX, actualTileObject.PosY].GetComponent<SpriteRenderer>().color = Color.green;
                    actualTileObject = TileObject;
                    tiles[actualTileObject.PosX, actualTileObject.PosY].GetComponent<SpriteRenderer>().color = Color.blue;
                    UpdateKimbokoTileInfoPanel(actualTileObject);
                }
            }
            else
            {
                actualTileObject = TileObject;
                tiles[actualTileObject.PosX, actualTileObject.PosY].GetComponent<SpriteRenderer>().color = Color.blue;
                UpdateKimbokoTileInfoPanel(actualTileObject);
            }
        }
        else
        {
            if (actualTileObject != null)
            {
                tiles[actualTileObject.PosX, actualTileObject.PosY].GetComponent<SpriteRenderer>().color = Color.green;
                actualTileObject = null;
                kimbokoInfoPanel.SetText(false);
            }
        }

    }

    private void SpawnInfoTest(SpawnAbilityEventInfo spawnInfo)
    {
        Debug.Log("Me Spawneo el Player: " + spawnInfo.spawnerPlayer.PlayerID);
        Debug.Log("Soy del tipo: " + spawnInfo.spawnUnitType);
        Debug.Log("Estoy en la Posicion: " + spawnInfo.spawnTile.PosX + "/" + spawnInfo.spawnTile.PosY);
    }

    private void UpdateKimbokoInfoPanel(HeatMapGridObject heatMapGridObject)
    {
        if (heatMapGridObject.GetEnemies().Count > 0)
        {
            string infoText = "HAS ENEMIES " + heatMapGridObject.GetEnemies().Count;
            //Debug.Log(infoText);
            kimbokoInfoPanel.SetText(true, infoText);
        }
        else
        {
            Debug.Log("NO ENEMIES ");
            kimbokoInfoPanel.SetText(false);
        }
    }

    private void UpdateKimbokoTileInfoPanel(Tile tileObject)
    {
        if (tileObject.IsOccupied())
        {
            string infoText = "IS OCCUPPY ";
            //Debug.Log(infoText);
            kimbokoInfoPanel.SetText(true, infoText);
            tileObject.GetOccupier().OnSelect(true, 0);
        }
        else
        {
            //Debug.Log("NO ENEMIES ");
            kimbokoInfoPanel.SetText(false);
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsMouseOverUIWithIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
            {
                raycastResultsList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultsList.Count > 0;
    }

    #endregion

    #region CARDS

    public void AddCard()
    {
        cardManager.AddCard(GetPlayer());

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










