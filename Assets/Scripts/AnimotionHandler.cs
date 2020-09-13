using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PositionerDemo;

public class AnimotionHandler : MonoBehaviour
{
    public enum TUTORIALOPTION { ONE, TWO, THREE};

    public TUTORIALOPTION tutorialOption;

    Camera cam;
    Grid<HeatMapGridObject> grid;
    UnitMovePositioner movePositioner;
    public float cellSize = 4f;

    public List<Transform> enemeyTransforms;
    public List<Enemy> enemies;
    public Enemy movingEnemy;
    public Enemy attackerEnemy;

    public GameObject Crane;
    public Transform CraneEnd;
    public GameObject kimbokoPrefab;


    MotionController motionControllerAttack = new MotionController();
    MotionController motionControllerSimpleMove = new MotionController();
    MotionController motionControllerCombineMove = new MotionController();
    MotionController motionControllerSpawn = new MotionController();

    PositionerDemo.Motion combMotion;
    PositionerDemo.Motion spawnMotion;

    private Vector2 startPosition;
    private Vector3 endPostion;
    private Vector3 finishPosition;
    bool va = true;

    void Start()
    {
        cam = Camera.main;
        grid = new Grid<HeatMapGridObject>(3, 3, cellSize, new Vector3(-5, -7), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        switch (tutorialOption)
        {
            case TUTORIALOPTION.ONE:
                StartMotionControllerOne();
                break;
            case TUTORIALOPTION.TWO:
                StartMotionControllerTwo();
                break;
            case TUTORIALOPTION.THREE:
                StartMotionControllerThree();
                break;
            default:
                break;
        }
    }

    private void StartMotionControllerOne()
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

        List<PositionerDemo.Motion> motionsMove = new List<PositionerDemo.Motion>();

        PositionerDemo.Motion motionMove = new MoveMotion(this, movingEnemy.GetComponent<Animator>(), 1);
        PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, movingEnemy.transform, 1, endPostion);

        motionsMove.Add(motionMove);
        motionsMove.Add(motionTweenMove);

        CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsMove);

        motionControllerSimpleMove.SetUpMotion(combinMoveMotion);
    }

    private void StartMotionControllerTwo()
    {
        movePositioner = new UnitMovePositioner(cellSize);
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

    private void StartMotionControllerThree()
    {
        //GameObject goKimbok = Instantiate(kimbokoPrefab);

        //spawnMotion = new SpawnMotion(this, Crane, goKimbok);

        //motionControllerSpawn.SetUpMotion(spawnMotion);
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
            else
            {
                Debug.Log("Is PErforming");
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
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, movingEnemy.transform, 1, endPostion);

                motionsMove.Add(motionMove);
                motionsMove.Add(motionTweenMove);

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
        // Necesitamos que los Transform reconozcan un input
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (heatMapGridObject != null)
            {
                if (motionControllerCombineMove != null && motionControllerCombineMove.isPerforming == false)
                {
                    //finalPositions = movePositioner.GetPositions(heatMapGridObject.GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));

                    Vector3 actualPosition = grid.GetGridObject(enemeyTransforms[0].position).GetRealWorldLocation();

                    Dictionary<Enemy, Vector3[]> enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies.ToArray(), movePositioner.GetPositionType(enemies.Count), heatMapGridObject.GetRealWorldLocation(), actualPosition);

                    List<PositionerDemo.Motion> motionsCombineMove = new List<PositionerDemo.Motion>();

                    int index = 0;

                    List<Configurable> configureAnimotion = new List<Configurable>();


                    // ANIM MOVE 1
                    // END PERFORM ANIM 1
                    // COMBINE ORDER 1
                    //                  TWEEN 1
                    //                  ANIM MOVE END 2

                    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
                    {

                        PositionerDemo.AnimatedMotion motionMove = new MoveMotion(this, entry.Key.GetComponent<Animator>(), 1);
                        PositionerDemo.Motion motionTweenMove = new MovePathTweenMotion(this, entry.Key.transform, 1, entry.Value);

                        KimbokoIdlleConfigureAnimotion<Animator, Transform> KimbokoPositionConfigAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(entry.Key.GetComponent<Animator>(), 2);

                        MotionIdlleConfigureAnimotion<AnimatedMotion, Transform> KimbokoAnimatedConfigAnimotion = new MotionIdlleConfigureAnimotion<AnimatedMotion, Transform>(motionMove, 1);


                        List<Configurable> extraConfigureAnimotion = new List<Configurable>();
                        List<PositionerDemo.Motion> extraMotionsCombineMove = new List<PositionerDemo.Motion>();

                        extraMotionsCombineMove.Add(motionTweenMove);
                        extraConfigureAnimotion.Add(KimbokoPositionConfigAnimotion);


                        CombineMotion extraCombinMoveMotion = new CombineMotion(this, 1, extraMotionsCombineMove, extraConfigureAnimotion);

                        motionsCombineMove.Add(motionMove);
                        motionsCombineMove.Add(extraCombinMoveMotion);
                        //motionsCombineMove.Add(motionTweenMove);
                        //configureAnimotion.Add(KimbokoPositionConfigAnimotion);
                        configureAnimotion.Add(KimbokoAnimatedConfigAnimotion);

                        index++;
                    }
                  
                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCombineMove, configureAnimotion);

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

                    GameObject goKimbok = Instantiate(kimbokoPrefab);


                    Vector3 craneStartPosition;
                    Vector3 craneEndPostion;

                    //A CRANE//GRUA SET ACTIVE = TRUE
                    Crane.SetActive(true);
                    //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
                    craneStartPosition = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Crane.transform.position.y, 0);
                    Crane.transform.position = craneStartPosition;
                    craneEndPostion = new Vector3(heatMapGridObject.GetRealWorldLocation().x, Helper.GetCameraTopBorderYWorldPostion().y);


                    // START //

                    PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, Crane.transform, 1, craneEndPostion);
                    motionsSpawn.Add(motionTweenMove);



                    // WAIT // 


                    ////C ANIMATION CRANESPAWNING
                    PositionerDemo.Motion motionCraneSpawn = new SpawnMotion(this, Crane.GetComponent<Animator>(), 2);
                    motionsSpawn.Add(motionCraneSpawn);



                    // WAIT //






                    // START CONFIGURE //



                    List<Configurable> configureAnimotion = new List<Configurable>();

                    KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, CraneEnd, 3);
                    configureAnimotion.Add(KimbokoPositionConfigAnimotion);

                    ////D INSTANTIATE KIMBOKO DESDE LA PUNTA DEL CRANE DONDE DEBERIA CHORREAR LA GOTA
                    //goKimbok.transform.position = CraneEnd.position;
                    //goKimbok.SetActive(true);

                    // END CONFIGURE //

                    ////E ANIMATION KIMBOKOSPAWNING
                    ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
                    ////F ANIMATION IDLLE... TAL VEZ SEA AUTOMATICO EL CAMBIO, PERO POR LAS DUDAS
                    PositionerDemo.Motion motionKimbokoSpawn = new SpawnMotion(this, kimbokoPrefab.GetComponent<Animator>(), 3);
                    PositionerDemo.Motion motionKimbokoTweenMove = new MoveTweenMotion(this, goKimbok.transform, 3, heatMapGridObject.GetRealWorldLocation());
                    motionsSpawn.Add(motionKimbokoSpawn);
                    motionsSpawn.Add(motionKimbokoTweenMove);



                    // WAIT //



                    //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
                    PositionerDemo.Motion motionTweenBackCraneMove = new MoveTweenMotion(this, Crane.transform, 4, craneStartPosition);
                    motionsSpawn.Add(motionTweenBackCraneMove);


                    // FINISH //

                    // START CONFIGURE //

                    ////H DESACTIVAMOS LA CRANE

                    CraneActiveConfigureAnimotion<Transform, Transform> craneActiveConfigAnimotion = new CraneActiveConfigureAnimotion<Transform, Transform>(Crane.transform, 5);
                    configureAnimotion.Add(craneActiveConfigAnimotion);

                    //Crane.SetActive(false);

                    // END CONFIGURE //


                    CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn);

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

}
