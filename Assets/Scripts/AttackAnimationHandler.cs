using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationHandler : MonoBehaviour
{
    private Animator animator; // Nuestro Animator
    public List<Enemy> enemies;
    IEnumerator actualAnimCoroutine;
    bool performing;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InputWithStopKeyPress();
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
            Debug.Log("Wait for End Animation Time Own Animator Check");
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
                Debug.Log("Wait for End Animation Time Enemies List Check");
                yield return null;
            }
        }

        performing = false;
        animator.speed = 1;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.speed = 1f;
        }
        Debug.Log("Animation has End ");
    }

    private void StartAttackDamageAnimation()
    {
        animator.SetTrigger("Active");

        animator.speed = 0.2f;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.speed = 0.2f;
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
        animator.speed = 1;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetTrigger("Idlle");
            enemies[i].animator.speed = 1f;
        }
    }

}
