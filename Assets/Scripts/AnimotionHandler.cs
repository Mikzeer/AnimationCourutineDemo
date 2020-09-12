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
        PositionerDemo.Motion motionAttack = new AttackMotion(this, enemies, attackerEnemy.GetComponent<Animator>());
        PositionerDemo.Motion motionMove = new MoveMotion(this, movingEnemy.GetComponent<Animator>());
        motionControllerAttack.SetUpMotion(motionAttack);
        motionControllerSimpleMove.SetUpMotion(motionMove);
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
        motionControllerAttack.TryReproduceMotion();
        motionControllerSimpleMove.TryReproduceMotion();
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

                    combMotion = new MoveCombineMotion(this, actualPosition, heatMapGridObject.GetRealWorldLocation(), enemies, cellSize);
                    motionControllerCombineMove.SetUpMotion(combMotion);
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
                    GameObject goKimbok = Instantiate(kimbokoPrefab);

                    spawnMotion = new SpawnMotion(this, Crane, goKimbok, heatMapGridObject.GetRealWorldLocation(), CraneEnd);

                    motionControllerSpawn.SetUpMotion(spawnMotion);
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
