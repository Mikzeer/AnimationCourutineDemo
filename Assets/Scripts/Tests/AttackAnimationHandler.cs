using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackAnimationHandler : MonoBehaviour
{
    private Animator animator; // Nuestro Animator
    public List<Enemy> enemies;
    IEnumerator actualAnimCoroutine;
    bool performing;

    bool cancel = false;

    [Range(0,2), SerializeField]
    private float animationSpeed = 2;

    private bool isPresing = false;
    private bool isFastForwarding = false;
    private float pressedTime = 0;
    private float timeToStarFastForward = 0.5f;

    IEnumerator actualMoveAnimCoroutine;
    public Enemy movingEnemy;
    private bool isMoving = false;
    private Vector2 startPosition;
    private Vector3 endPostion;

    [SerializeField] private Ease ease = Ease.Linear;

    [Range(1.0f, 4.0f), SerializeField]
    private float moveDuration = 4.0f;

    [SerializeField]
    private DOTween dorr;

    private Tween movingTween;

    PositionerDemo.MotionController motionControler = new PositionerDemo.MotionController();
    PositionerDemo.MotionController motionControlerTwo = new PositionerDemo.MotionController();

    void Awake()
    {
        animator = GetComponent<Animator>();


        startPosition = movingEnemy.GetComponent<Transform>().position;
        endPostion = startPosition + new Vector2(0, 15);


        PositionerDemo.Motion motionAttack = new PositionerDemo.AttackMotion(this, enemies, animator);
        PositionerDemo.Motion motionMove = new PositionerDemo.MoveMotion(this, movingEnemy.GetComponent<Animator>());
        motionControler.SetUpMotion(motionAttack);
        motionControlerTwo.SetUpMotion(motionMove);
    }

    private void Update()
    {
        //InputWithStopKeyPress();
        //KeepButtonPress();
        //InputStartMoving();
        motionControler.TryReproduceMotion();
        motionControlerTwo.TryReproduceMotion();
        //motionControler.SpeedUpOnButtonPress();
        //motionControlerTwo.SpeedUpOnButtonPress();
    }




    public void InputWithStopKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!performing)
            {
                if (actualAnimCoroutine != null)
                {
                    StopCoroutine(actualAnimCoroutine);
                    actualAnimCoroutine = PlayAttackAnimations();
                    StartCoroutine(actualAnimCoroutine);
                }
                else
                {
                    actualAnimCoroutine = PlayAttackAnimations();
                    StartCoroutine(actualAnimCoroutine);
                }
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }
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





    public IEnumerator PlayAttackAnimations()
    {
        performing = true;
        cancel = false;

        StartAttackDamageAnimation();

        AnimatorStateInfo animst = animator.GetCurrentAnimatorStateInfo(0);
        // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
        yield return null;

        // Esto es para chequear el final de la animacion presionando un boton
        StartCoroutine(EndAnimationOnButtonPress());

        // Esperamos a terminar de hacer la animacion y volver a Iddle tanto de ataque como de recibir dano de los enemigos
        yield return CheckIfStop(animst);

        OnEndAttackAnimation();
    }

    private void StartAttackDamageAnimation()
    {
        animator.SetTrigger("Active");

        //animator.speed = 0.2f;
        if (!isFastForwarding) animator.SetFloat("AnimationSpeed", 0.2f);

        for (int i = 0; i < enemies.Count; i++)
        {
            //enemies[i].animator.speed = 0.2f;
            if (!isFastForwarding) enemies[i].animator.SetFloat("AnimationSpeed", 0.2f);
            enemies[i].animator.SetTrigger("Damage");
        }
    }

    IEnumerator EndAnimationOnButtonPress()
    {
        bool done = false;
        while (!done)
        {
            if (performing == false)
            {
                done = true;
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (performing == true)
                {
                    cancel = true;
                    done = true;
                    EndAttackDamageAnimation();
                    yield break;
                }
            }
            yield return null;
        }

        yield return null;      
    }

    private void EndAttackDamageAnimation()
    {
        animator.SetTrigger("Idlle");
        //animator.speed = 1;
        if (!isFastForwarding) animator.SetFloat("AnimationSpeed", 1f);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetTrigger("Idlle");
            if (!isFastForwarding) enemies[i].animator.SetFloat("AnimationSpeed", 1f);
            //enemies[i].animator.speed = 1f;
        }
    }

    public IEnumerator CheckIfStop(AnimatorStateInfo animst)
    {
        while (animst.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            //Debug.Log("Wait for End Animation Time Own Animator Check");
            yield return null;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            // Tambien esperamos que termine la animacion de Damage de los enemigos
            while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
            {
                //Debug.Log("Wait for End Animation Time Enemies List Check");
                yield return null;
            }
        }

        //while (animst.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //{
        //    //Debug.Log("Wait for End Animation Time Own Animator Check");
        //    yield return null;
        //}

        ////LA FORMA DE HACERLO CON UN STRING
        //while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        //{
        //    Debug.Log("Wait for End Animation Time Own Animator Check");
        //    yield return null;
        //}

        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    // Tambien esperamos que termine la animacion de Damage de los enemigos
        //    while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        //    {
        //        //Debug.Log("Wait for End Animation Time Enemies List Check");
        //        yield return null;
        //    }
        //}
    }

    private void OnEndAttackAnimation()
    {
        if (cancel == false)
        {
            animator.SetFloat("AnimationSpeed", 1f);
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].animator.SetFloat("AnimationSpeed", 1f);
            }
            performing = false;
        }
        else
        {
            cancel = false;
            performing = false;
        }
    }



    private void SpeedUpAnimation()
    {
        if (!isMoving) return;

        moveDuration = 5;

        movingTween.timeScale = moveDuration;

        animator.SetFloat("AnimationSpeed", animationSpeed);
        movingEnemy.animator.SetFloat("AnimationSpeed", animationSpeed);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetFloat("AnimationSpeed", animationSpeed);
        }
    }

    private void SetAnimationSpeedToNormal()
    {
        if (movingTween == null) return;

        moveDuration = 1;
        movingTween.timeScale = moveDuration;

        animator.SetFloat("AnimationSpeed", 1);
        movingEnemy.animator.SetFloat("AnimationSpeed", 1);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetFloat("AnimationSpeed", 1);
        }
    }




    public void InputStartMoving()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isMoving == false)
            {
                if (actualMoveAnimCoroutine != null)
                {
                    StopCoroutine(actualMoveAnimCoroutine);
                    actualMoveAnimCoroutine = PlayMoveAnimations();
                    StartCoroutine(actualMoveAnimCoroutine);
                }
                else
                {
                    actualMoveAnimCoroutine = PlayMoveAnimations();
                    StartCoroutine(actualMoveAnimCoroutine);
                }
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }
    }

    private IEnumerator PlayMoveAnimations()
    {
        Vector2 oldPosition = startPosition;

        isMoving = true;

        StartMoveAnimation();

        startPosition = endPostion;
        endPostion = oldPosition;

        AnimatorStateInfo animst = movingEnemy.animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log("First " + animst.shortNameHash);
        // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
        // la transition duration entre los animation states debe ser 0.0f para poder funcionar con este codigo
        yield return null;

        //yield return new WaitForSeconds(0.2f);
        // Esto es para chequear el final de la animacion presionando un boton
        StartCoroutine(EndMoveAnimationOnButtonPress(startPosition));

        // ESTA ES UNA DE LAS CLAUSULAS PARA TERMINAR LA ANIMACION
        // Esperamos a terminar de hacer la animacion y volver a Iddle
        //while (animst.shortNameHash != movingEnemy.animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //{
        //    //Debug.Log("Wait for End Animation Time Own Animator Check");
        //    yield return null;
        //}

        while (movingEnemy.transform.position != endPostion)
        {
            Debug.Log("Wait for End Animation Time Own Animator Check");
            yield return null;
        }

        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    // Tambien esperamos que termine la animacion de Damage de los enemigos
        //    while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        //    {
        //        //Debug.Log("Wait for End Animation Time Enemies List Check");
        //        yield return null;
        //    }
        //}

        isMoving = false;
        //animator.speed = 1;
        movingEnemy.animator.SetFloat("AnimationSpeed", 1f);
        movingEnemy.animator.SetTrigger("Idlle");
       
        Debug.Log("Animation has End ");
    }

    private void StartMoveAnimation()
    {
        if (!isFastForwarding) movingEnemy.animator.SetFloat("AnimationSpeed", 0.2f);
        movingEnemy.animator.SetTrigger("Move");
        movingTween = movingEnemy.transform.DOMove(endPostion, 20).SetEase(ease);
    }

    IEnumerator EndMoveAnimationOnButtonPress(Vector2 finishPosition)
    {
        bool done = false;

        while (!done)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("ENDED ALL WITH SPACE");
                StopCoroutine(actualMoveAnimCoroutine);
                done = true;
                isMoving = false;
                movingEnemy.animator.SetTrigger("Idlle");

                movingTween.Kill();

                movingEnemy.transform.position = finishPosition;

                //animator.speed = 1;
                if (!isFastForwarding) movingEnemy.animator.SetFloat("AnimationSpeed", 1f);
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



    private void RestTrigger(string animation)
    {
        //Resets the value of the given trigger parameter.Use this to reset a Trigger parameter in an Animator Controller that could still be active. 
        //Make sure to create a parameter in the Animator Controller with the same name.See Animator.SetTrigger for more information about how to set a Trigger.
        //Reset the animation trigger
        animator.ResetTrigger(animation);
    }

}

