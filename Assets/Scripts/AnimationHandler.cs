using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    enum ANIMATIONSTATE { IDDLE, ATTACK, DAMAGE};
    Animator animator;
    IEnumerator actualAnimCoroutine;
    bool performing;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
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
                    PlayAnimation(ANIMATIONSTATE.ATTACK);
                    actualAnimCoroutine = EndAnimationOnButtonPress();
                    StartCoroutine(actualAnimCoroutine);
                }
                else
                {
                    PlayAnimation(ANIMATIONSTATE.ATTACK);
                    actualAnimCoroutine = EndAnimationOnButtonPress();
                    StartCoroutine(actualAnimCoroutine);
                }
            }
            else
            {
                Debug.Log("Is Performing animation");
            }

        }
    }

    private void PlayAnimation(ANIMATIONSTATE animState)
    {
        switch (animState)
        {
            case ANIMATIONSTATE.IDDLE:
                animator.SetTrigger("Idlle");
                break;
            case ANIMATIONSTATE.ATTACK:
                animator.SetTrigger("Active");
                break;
            case ANIMATIONSTATE.DAMAGE:
                animator.SetTrigger("Damage");
                break;
            default:
                break;
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
            yield return null;
        }
        animator.SetTrigger("Idlle");
        Debug.Log("Animation has End ");
        performing = false;
        yield return null;
    }
}
