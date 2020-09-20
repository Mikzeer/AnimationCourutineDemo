using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;

public class AnimotionHandler : MonoBehaviour
{
    public enum TUTORIALOPTION { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN};
    public TUTORIALOPTION tutorialOption;
    public float cellSize = 4f;
    public int withd = 5;
    public int height = 7;
    public List<Transform> enemeyTransforms;
    public List<Enemy> enemies;
    public Enemy movingEnemy;
    public Enemy attackerEnemy;
    public GameObject Crane;
    public Transform CraneEnd;
    public GameObject kimbokoPrefab;
    public GameObject shieldPrefab;
    public GameObject tilePrefab;
    public RectTransform bannerRect;
    public RectTransform bannerRectTimer;

    Camera cam;
    Grid<HeatMapGridObject> grid;
    UnitMovePositioner movePositioner;

    MotionController motionControllerCreateBoard = new MotionController();
    MotionController motionControllerAttack = new MotionController();
    MotionController motionControllerSimpleMove = new MotionController();
    MotionController motionControllerCombineMove = new MotionController();
    MotionController motionControllerSpawn = new MotionController();
    MotionController motionControllerCombineSpawn = new MotionController();
    MotionController motionControllerBanner = new MotionController();
    MotionController motionControllerBannerTimmer = new MotionController();
    MotionController motionControllerCombineSpawnWithCheck = new MotionController();

    private Vector2 startPosition;
    private Vector3 endPostion;
    private Vector3 finishPosition;
    bool va = true;

    private GameObject[,] tiles;

    void Start()
    {
        cam = Camera.main;
        grid = new Grid<HeatMapGridObject>(withd, height, cellSize, new Vector3(-10, -10), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        movePositioner = new UnitMovePositioner(cellSize);
        SetBoard();

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
    }

    private void SetBoard()
    {
        tiles = new GameObject[withd, height];

        List<PositionerDemo.Motion> motionsCreateBoard = new List<PositionerDemo.Motion>();

        int index = 1;

        for (int x = 0; x < withd; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 thisTileFinalPosition = grid.GetGridObject(x, y).GetRealWorldLocation();

                tiles[x, y] = Instantiate(tilePrefab);
                tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                tiles[x, y].transform.localScale *= cellSize; 
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

    private void StartMotionControllerTwo()
    {
        Vector3[]  finalPositions = movePositioner.GetPositions(grid.GetGridObject(0, 0).GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));

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

    void Update()
    {
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
            default:
                break;
        }
    }

    private void UpdateControllerOne()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (motionControllerAttack != null && motionControllerAttack.isPerforming == false)
            {
                List<PositionerDemo.Motion> motions = new List<PositionerDemo.Motion>();
                PositionerDemo.Motion motionAttack = new AttackMotion(this, attackerEnemy.GetComponent<Animator>(), 1);

                motions.Add(motionAttack);

                for (int i = 0; i < enemies.Count; i++)
                {
                    PositionerDemo.Motion motionDamage = new DamageMotion(this, enemies[i].animator, 1);
                    motions.Add(motionDamage);
                }

                CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
                motionControllerAttack.SetUpMotion(combineAttackMotion);

                motionControllerAttack.TryReproduceMotion();
            }

            if (motionControllerSimpleMove != null && motionControllerSimpleMove.isPerforming == false)
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

                List<PositionerDemo.Motion> motionsStopMove = new List<PositionerDemo.Motion>();
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, movingEnemy.transform, 1, endPostion);
                PositionerDemo.Motion motionIdlle = new IdlleMotion(this, movingEnemy.GetComponent<Animator>(), 2, true);
                motionsStopMove.Add(motionTweenMove);
                motionsStopMove.Add(motionIdlle);

                CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsStopMove);
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
                if (motionControllerCombineMove != null && motionControllerCombineMove.isPerforming == false)
                {
                    Vector3 actualPosition = grid.GetGridObject(enemeyTransforms[0].position).GetRealWorldLocation();
                    Dictionary<Enemy, Vector3[]> enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies.ToArray(), movePositioner.GetPositionType(enemies.Count), heatMapGridObject.GetRealWorldLocation(), actualPosition);
                    List<PositionerDemo.Motion> motionsCombineMove = new List<PositionerDemo.Motion>();
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

                        CombineMotion extraCombineMoveStopMotion = new CombineMotion(this, 1, extraMotionsCombineStopMove);
                        motionsCombineMove.Add(extraCombineMoveStopMotion);
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
                if (motionControllerSpawn != null && motionControllerSpawn.isPerforming == false)
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
                if (motionControllerCombineSpawn != null && motionControllerCombineSpawn.isPerforming == false)
                {
                    List<PositionerDemo.Motion> motionsSpawnCombine = new List<PositionerDemo.Motion>();
                    List<Configurable> configureAnimotion = new List<Configurable>();

                    // ACA TENEMOS QUE VER SI HAY MAS DE UNA UNIDAD EN LA TILE
                    // MIENTRAS BAJA EL CRANE // UNIDADES EN LA TILE SE REPOSICIONAN A LOS COSTADOS

                    // ACA TENGO LA POSICION DE LA TILA DONDE DEBERIAN ESTAR LAS UNIDADES
                    Vector3 actualPosition = heatMapGridObject.GetRealWorldLocation();
                    // AHORA TENGO QUE GENERAR EL CUADRADO DE POSICIONES PARA CADA UNA
                    Vector3[] squarePositions = movePositioner.GetPositions(actualPosition, POSITIONTYPE.SQUARE);

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

                        CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);

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
            if (motionControllerAttack != null && motionControllerAttack.isPerforming == false)
            {
                List<PositionerDemo.Motion> motions = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();

                PositionerDemo.Motion motionAttack = new AttackMotion(this, attackerEnemy.GetComponent<Animator>(), 1);
                motions.Add(motionAttack);

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
            if (motionControllerBanner != null && motionControllerBanner.isPerforming == false)
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

            if (motionControllerBannerTimmer != null && motionControllerBannerTimmer.isPerforming == false)
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

                if (motionControllerCombineSpawnWithCheck != null && motionControllerCombineSpawnWithCheck.isPerforming == false)
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

                            CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                            motionsCombineSpawnMoveSquare.Add(combineStopMotion);

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

                        // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                        for (int i = 0; i < kimbokoToRepositionSquare.Count; i++)
                        {
                            List<PositionerDemo.Motion> motionsCombineSpawnMoveSquare = new List<PositionerDemo.Motion>();

                            int shortNameHash = Animator.StringToHash("Base Layer" + ".Idlle");
                            Debug.Log("shortNameHash " + shortNameHash);

                            PositionerDemo.Motion motionMove = new MoveMotion(this, kimbokoToRepositionSquare[i].animator, 1, false , shortNameHash);
                            motionsCombineSpawnMoveSquare.Add(motionMove);

                            List<PositionerDemo.Motion> motionsCombineSpawnStopMoveSquare = new List<PositionerDemo.Motion>();

                            PositionerDemo.Motion motionTwMove = new MoveTweenMotion(this, kimbokoToRepositionSquare[i].transform, 1, finalRearrangePositions[i], 1);
                            motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                            PositionerDemo.Motion motionIdlle = new IdlleMotion(this, kimbokoToRepositionSquare[i].animator, 2, true);
                            motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                            CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                            motionsCombineSpawnMoveSquare.Add(combineStopMotion);

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
                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
    }

}