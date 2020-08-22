using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;
    IEnumerator actualAnimCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
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


    IEnumerator CheckIfAnimationHasEnd(string animTrigerName)
    {
        animator.SetTrigger(animTrigerName);



        yield return null;
    }

}
