using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class PreserveAnimatorOnDisable : MonoBehaviour
    {
        /*
         * by the time OnDisable is called, the anim.parameters array is empty (probably another bug), 
         * so you actually have to call OnAnimDisable before disabling the object. 
         * Then, the state will be saved and restored OnEnable. 
         * Just attach this script to the object with the Animator on it. 
         * I don't know if this will work for triggers though, as I don't use any in my project. 
         * It works for bools, floats and ints though.
         */

        private class AnimParam
        {
            public AnimatorControllerParameterType type;
            public string paramName;
            object data;

            public AnimParam(Animator anim, string paramName, AnimatorControllerParameterType type)
            {
                this.type = type;
                this.paramName = paramName;
                switch (type)
                {
                    case AnimatorControllerParameterType.Int:
                        this.data = (int)anim.GetInteger(paramName);
                        break;
                    case AnimatorControllerParameterType.Float:
                        this.data = (float)anim.GetFloat(paramName);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        this.data = (bool)anim.GetBool(paramName);
                        break;
                }
            }
            public object getData()
            {
                return data;
            }
        }

        Animator anim;
        List<AnimParam> parms = new List<AnimParam>();
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void OnAnimDisable()
        {
            Debug.Log("Saving Animator state: " + anim.parameters.Length);
            for (int i = 0; i < anim.parameters.Length; i++)
            {
                AnimatorControllerParameter p = anim.parameters[i];
                AnimParam ap = new AnimParam(anim, p.name, p.type);
                parms.Add(ap);
            }
        }

        void OnEnable()
        {
            Debug.Log("Restoring Animator state.");
            foreach (AnimParam p in parms)
            {
                switch (p.type)
                {
                    case AnimatorControllerParameterType.Int:
                        anim.SetInteger(p.paramName, (int)p.getData());
                        break;
                    case AnimatorControllerParameterType.Float:
                        anim.SetFloat(p.paramName, (float)p.getData());
                        break;
                    case AnimatorControllerParameterType.Bool:
                        anim.SetBool(p.paramName, (bool)p.getData());
                        break;
                }
            }
            parms.Clear();
        }

        /*
 * For the Animator system 
 * This is the new Unity animation playback system. This should be used in your new Project instead of the Animation API.
 * In terms of performance, it's better to use the Animator.StringToHash and compare the current state by hash number instead 
 * of the IsName function which compares string since the hash is faster.
 * Let's say that you have state names called Jump, Move and Look. 
 * You get their hashes as below then use the function for playing and waiting for them them below:
 */
        const string animBaseLayer = "Base Layer";
        int jumpAnimHash = Animator.StringToHash(animBaseLayer + ".Jump");
        int moveAnimHash = Animator.StringToHash(animBaseLayer + ".Move");
        int lookAnimHash = Animator.StringToHash(animBaseLayer + ".Look");

        public IEnumerator PlayAndWaitForAnim(Animator targetAnim, string stateName)
        {
            //Get hash of animation
            int animHash = 0;
            if (stateName == "Jump")
                animHash = jumpAnimHash;
            else if (stateName == "Move")
                animHash = moveAnimHash;
            else if (stateName == "Look")
                animHash = lookAnimHash;

            //targetAnim.Play(stateName);
            targetAnim.CrossFadeInFixedTime(stateName, 0.6f);

            //Wait until we enter the current state
            while (targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
            {
                yield return null;
            }

            float counter = 0;
            float waitTime = targetAnim.GetCurrentAnimatorStateInfo(0).length;

            //Now, Wait until the current state is done playing
            while (counter < (waitTime))
            {
                counter += Time.deltaTime;
                yield return null;
            }

            //Done playing. Do something below!
            Debug.Log("Done Playing");

        }

    }

}



