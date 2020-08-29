using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationHandler : MonoBehaviour
{
    private Animator animator; // Nuestro Animator
    public List<Enemy> enemies;
    IEnumerator actualAnimCoroutine;
    bool performing;

    [Range(0,2)]
    [SerializeField]private float animationSpeed = 2;
    private bool isPresing = false;
    private bool isFastForwarding = false;
    private float pressedTime = 0;
    private float timeToStarFastForward = 0.5f;

    IEnumerator actualMoveAnimCoroutine;
    public Enemy movingEnemy;
    private bool isMoving = false;
    private Vector2 startPosition;
    private Vector2 endPostion;


    void Awake()
    {
        animator = GetComponent<Animator>();
        startPosition = movingEnemy.GetComponent<Transform>().position;
        endPostion = startPosition + new Vector2(10, 0);
    }

    private void Update()
    {
        InputWithStopKeyPress();
        KeepButtonPress();
        InputStartMoving();
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
        isMoving = true;

        StartMoveAnimation();

        AnimatorStateInfo animst = movingEnemy.animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log("First " + animst.shortNameHash);
        // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
        // la transition duration entre los animation states debe ser 0.0f para poder funcionar con este codigo
        yield return null;

        //yield return new WaitForSeconds(0.2f);
        // Esto es para chequear el final de la animacion presionando un boton
        StartCoroutine(EndMoveAnimationOnButtonPress());

        // ESTA ES UNA DE LAS CLAUSULAS PARA TERMINAR LA ANIMACION
        // Esperamos a terminar de hacer la animacion y volver a Iddle
        while (animst.shortNameHash != movingEnemy.animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            //Debug.Log("Wait for End Animation Time Own Animator Check");
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

        Debug.Log("Animation has End ");
    }

    private void StartMoveAnimation()
    {
        if (!isFastForwarding) movingEnemy.animator.SetFloat("AnimationSpeed", 0.2f);
        movingEnemy.animator.SetTrigger("Move");
    }

    IEnumerator EndMoveAnimationOnButtonPress()
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




    public IEnumerator PlayAttackAnimations()
    {
        performing = true;

        StartAttackDamageAnimation();

        AnimatorStateInfo animst = animator.GetCurrentAnimatorStateInfo(0);

        // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
        yield return null;

        // Esto es para chequear el final de la animacion presionando un boton
        StartCoroutine(EndAnimationOnButtonPress());

        // Esperamos a terminar de hacer la animacion y volver a Iddle

        while (animst.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            //Debug.Log("Wait for End Animation Time Own Animator Check");
            yield return null;
        }

        // LA FORMA DE HACERLO CON UN STRING
        //while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        //{
        //    Debug.Log("Wait for End Animation Time Own Animator Check");
        //    yield return null;
        //}

        for (int i = 0; i < enemies.Count; i++)
        {
            // Tambien esperamos que termine la animacion de Damage de los enemigos
            while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
            {
                //Debug.Log("Wait for End Animation Time Enemies List Check");
                yield return null;
            }
        }

        performing = false;
        //animator.speed = 1;
        animator.SetFloat("AnimationSpeed", 1f);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetFloat("AnimationSpeed", 1f);
            //enemies[i].animator.speed = 1f;
        }
        Debug.Log("Animation has End ");
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("ENDED ALL WITH SPACE");
                StopCoroutine(actualAnimCoroutine);
                done = true;
                performing = false;
                EndAttackDamageAnimation();
                yield return null;
            }

            if (performing == false)
            {
                done = true;
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

    private void SpeedUpAnimation()
    {
        animator.SetFloat("AnimationSpeed", animationSpeed);
        movingEnemy.animator.SetFloat("AnimationSpeed", animationSpeed);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetFloat("AnimationSpeed", animationSpeed);
        }
    }

    private void SetAnimationSpeedToNormal()
    {
        animator.SetFloat("AnimationSpeed", 1);
        movingEnemy.animator.SetFloat("AnimationSpeed", 1);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetFloat("AnimationSpeed", 1);
        }
    }


    //Resets the value of the given trigger parameter.Use this to reset a Trigger parameter in an Animator Controller that could still be active. 
    //Make sure to create a parameter in the Animator Controller with the same name.See Animator.SetTrigger for more information about how to set a Trigger.
    private void RestTrigger(string animation)
    {
        //Reset the animation trigger
        animator.ResetTrigger(animation);
    }
}

