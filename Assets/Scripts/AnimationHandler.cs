using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;
    IEnumerator actualAnimCoroutine;
    bool performing;
    // OBJETIVO = REPRODUCIR UNA ANIMACION QUE DURE DETERMINADO TIEMPO, Y DETENERLA ANTES DE QUE TERMINE 
    // INICIAR UNA COROUTINE CON EL NOMBRE DE LA ANIMACION
    // INICIAR UN TIMER PARA CORRER UN TIEMPO DE DURACION DE LOOPEO DE LA ANIMACION

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //InputUpdateTest();
        InputWithStopKeyPress();
    }

    public void InputUpdateTest()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            if (actualAnimCoroutine != null)
            {
                StopCoroutine(actualAnimCoroutine);
                actualAnimCoroutine = CheckIfAnimationHasEnd("Active");
                StartCoroutine(actualAnimCoroutine);
            }
            else
            {
                actualAnimCoroutine = CheckIfAnimationHasEnd("Active");
                StartCoroutine(actualAnimCoroutine);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (actualAnimCoroutine != null)
            {
                StopCoroutine(actualAnimCoroutine);
                StartCoroutine(CheckIfAnimationHasEnd("Idlle"));
                StartCoroutine(actualAnimCoroutine);
            }
            else
            {
                StartCoroutine(CheckIfAnimationHasEnd("Idlle"));
                StartCoroutine(actualAnimCoroutine);
            }

        }
    }

    public void InputWithStopKeyPress()
    {
        if (!performing)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (actualAnimCoroutine != null)
                {
                    StopCoroutine(actualAnimCoroutine);
                    animator.SetTrigger("Active");
                    actualAnimCoroutine = EndAnimationOnButtonPress();
                    StartCoroutine(actualAnimCoroutine);
                }
                else
                {
                    animator.SetTrigger("Active");
                    actualAnimCoroutine = EndAnimationOnButtonPress();
                    StartCoroutine(actualAnimCoroutine);
                }
            }
        }
    }

    IEnumerator CheckIfAnimationHasEnd(string animTrigerName)
    {
        animator.SetTrigger(animTrigerName);

        
        yield return EndAnimationOnButtonPress();
    }

    IEnumerator WaitForAKeyToBePress(KeyCode key)
    {
        // time delay
        yield return new WaitForSeconds(2.5f);

        bool done = false;

        while (!done)
        {
            if (Input.GetKeyDown(key))
            {
                done = true;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
            {
                done = true;
            }

            // wait until next frame, then continue the execution
            yield return null;                 
        }        
    }

    IEnumerator EndAnimationOnButtonPress()
    {
        float timer = 5;
        performing = true;
        bool done = false;

        while (!done)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                done = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                done = true;
            }

            Debug.Log("Wait for End Animation Time ");
        }

        Debug.Log("Animation has End ");
        performing = false;
        yield return null;
    }
}
