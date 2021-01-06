using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace PositionerDemo
{

    public class TestPositioner : MonoBehaviour
    {
        private bool isPresing = false;
        private float pressedTime = 0;
        private float timeToStarFastForward = 0.5f;
        [Range(1.0f, 4.0f), SerializeField]
        private float moveDuration = 4.0f;
        [Range(0, 2), SerializeField]
        private float animationSpeed = 2;

        public List<Enemy> enemies;
        public List<Transform> enemeyTransforms;

        Camera cam;
        Grid<HeatMapGridObject> grid;

        UnitMovePositioner movePositioner;

        public float cellSize = 4f;

        Vector3[] finalPositions;


        [SerializeField] private Ease ease = Ease.Linear;
        //IEnumerator actualMoveAnimCoroutine;
        private bool isMoving = false;
        public Enemy movingEnemy;
        private Tween[] movingTween;
        private bool isFastForwarding = false;
        Dictionary<Enemy, Vector3[]> enmiesAndPathToMove;
        private Vector3 endPostion;
        Vector3 actualPosition;

        MotionController motControl = new MotionController();
        Motion combMotion;

        void Start()
        {
            cam = Camera.main;
            grid = new Grid<HeatMapGridObject>(3, 3, cellSize, new Vector3(-5, -5), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

            movePositioner = new UnitMovePositioner(cellSize);
            movingTween = new Tween[enemeyTransforms.Count];
            finalPositions = movePositioner.GetPositions(grid.GetGridObject(0, 0).GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));
            actualPosition = grid.GetGridObject(0, 0).GetRealWorldLocation();
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
            // Necesitamos que los Transform reconozcan un input
            if (Input.GetMouseButtonDown(0))
            {
                HeatMapGridObject heatMapGridObject = grid.GetGridObject(GetMouseWorldPosition());
                if (heatMapGridObject != null)
                {                 
                    if (motControl != null && motControl.IsPerforming == false)
                    {
                        finalPositions = movePositioner.GetPositions(heatMapGridObject.GetRealWorldLocation(), movePositioner.GetPositionType(enemeyTransforms.Count));

                        actualPosition = grid.GetGridObject(enemeyTransforms[0].position).GetRealWorldLocation();

                        combMotion = new MoveCombineMotion(this, actualPosition, heatMapGridObject.GetRealWorldLocation(), enemies, cellSize);
                        motControl.SetUpMotion(combMotion);
                        motControl.TryReproduceMotion();


                        //if (actualMoveAnimCoroutine != null)
                        //{
                        //    StopCoroutine(actualMoveAnimCoroutine);
                        //    actualMoveAnimCoroutine = PlayMoveAnimations(actualPosition, heatMapGridObject.GetRealWorldLocation());
                        //    StartCoroutine(actualMoveAnimCoroutine);
                        //}
                        //else
                        //{
                        //    actualMoveAnimCoroutine = PlayMoveAnimations(actualPosition, heatMapGridObject.GetRealWorldLocation());
                        //    StartCoroutine(actualMoveAnimCoroutine);
                        //}
                    }
                    else
                    {
                        Debug.Log("Is Performing animation");
                    }
                }
            }

            //motControl.SpeedUpOnButtonPress();
            //KeepButtonPress();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, cam);
            vec.z = 0f;
            return vec;
        }

        private Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        private IEnumerator PlayMoveAnimations(Vector3 startPosition, Vector3 finalPosition)
        {
            isMoving = true;

            endPostion = finalPosition;

            StartMoveAnimation(startPosition, finalPosition);

            // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
            // la transition duration entre los animation states debe ser 0.0f para poder funcionar con este codigo
            yield return null;

            // Esto es para chequear el final de la animacion presionando un boton
            StartCoroutine(EndMoveAnimationOnButtonPress());

            foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
            {
                while (entry.Key.transform.position != entry.Value[2])
                {
                    Debug.Log("Wait for End Animation Time Own Animator Check");
                    yield return null;
                }


                if (!isFastForwarding) entry.Key.animator.SetFloat("AnimationSpeed", 1f);
                entry.Key.animator.SetTrigger("Idlle");

            }

            isMoving = false;
            actualPosition = finalPosition;
            Debug.Log("Animation has End ");
        }

        private void StartMoveAnimation(Vector3 startPosition, Vector3 finalPosition)
        {            
            enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies.ToArray(), movePositioner.GetPositionType(enemies.Count), finalPosition, startPosition);

            int index = 0;
            foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
            {
                float duration = moveDuration;

                if (!isFastForwarding)
                {
                    entry.Key.animator.SetFloat("AnimationSpeed", 0.5f);
                    duration = 1;
                }

                entry.Key.animator.SetTrigger("Move");

                movingTween[index] = entry.Key.transform.DOPath(entry.Value, 10).SetEase(ease);
                movingTween[index].timeScale = duration;

                index++;
                // do something with entry.Value or entry.Key
            }
        }

        IEnumerator EndMoveAnimationOnButtonPress()
        {
            bool done = false;

            while (!done)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("ENDED ALL WITH SPACE");
                    //StopCoroutine(actualMoveAnimCoroutine);
                    done = true;
                    isMoving = false;

                    actualPosition = endPostion;

                    int index = 0;
                    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
                    {
                        //animator.speed = 1;
                        if (!isFastForwarding) entry.Key.animator.SetFloat("AnimationSpeed", 1f);
                        entry.Key.animator.SetTrigger("Idlle");
                        movingTween[index].Kill();
                        index++;
                        entry.Key.transform.position = entry.Value[2];


                        // do something with entry.Value or entry.Key
                    }

                    

                    yield return null;
                }

                if (isMoving == false)
                {
                    done = true;
                }

                yield return null;
            }

            yield return null;
        }

        public void KeepButtonPress()
        {
            if (Input.GetKey(KeyCode.W))
            {
                isPresing = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                isPresing = false;
                pressedTime = 0;
                if (isFastForwarding)
                {
                    SetAnimationSpeedToNormal();
                    isFastForwarding = false;
                }
            }

            if (isPresing)
            {
                if (isFastForwarding == false)
                {
                    // Esto lo tengo que hacer si todavia no comenzo a fastforwardear
                    pressedTime += Time.deltaTime;

                    Debug.Log("Pressed Time " + Mathf.FloorToInt(pressedTime));
                    if (pressedTime >= timeToStarFastForward)
                    {
                        Debug.Log("Time achive");
                        isFastForwarding = true;
                        SpeedUpAnimation();
                    }
                }
            }
        }

        private void SetAnimationSpeedToNormal()
        {
            moveDuration = 1;

            int index = 0;
            foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
            {
                movingTween[index].timeScale = moveDuration;
                entry.Key.animator.SetFloat("AnimationSpeed", 1);
                index++;
            }
        }

        private void SpeedUpAnimation()
        {
            moveDuration = 5;

            int index = 0;
            foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
            {
                movingTween[index].timeScale = moveDuration;
                entry.Key.animator.SetFloat("AnimationSpeed", animationSpeed);
                index++;
            }
        }

    }

}

