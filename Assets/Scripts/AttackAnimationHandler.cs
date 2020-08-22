using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationHandler : MonoBehaviour
{
    private Animator animator; // Nuestro Animator
    private bool waitingForTargetDamageAnimation; // Nos sirve para saber cuando se termino la animacion
    public List<Enemy> enemies;

    enum ANIMATIONSTATE { IDDLE, ATTACK, DAMAGE };
    IEnumerator actualAnimCoroutine;
    bool performing;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
        waitingForTargetDamageAnimation = true;

        animator.SetTrigger("Active");

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].animator.SetTrigger("Damage");
        }

        // Esperamos a terminar de hacer la animacion y volver a Iddle
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        {
            yield return null;
        }

        for (int i = 0; i < enemies.Count; i++)
        {            
            // Tambien esperamos que termine la animacion de dano de los enemigos
            while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Iddle"))
            {
                yield return null;
            }
        }

        performing = false;
        //animator.SetTrigger("Idlle");
    }

    private void DamageTarget(Enemy target)
    {
        StartCoroutine(TriggerTargetDamageAnimation(target));
    }

    private IEnumerator TriggerTargetDamageAnimation(Enemy target)
    {
        yield return StartCoroutine(PlayDamageAnimation(target));

        waitingForTargetDamageAnimation = false;
    }

    private IEnumerator PlayDamageAnimation(Enemy target)
    {
        target.animator.SetTrigger("Damage");

        while (!target.animator.GetCurrentAnimatorStateInfo(0).IsName("Iddle"))
        {
            yield return null;
        }
    }

}
