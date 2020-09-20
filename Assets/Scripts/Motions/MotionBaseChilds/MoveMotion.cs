using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace PositionerDemo
{
    public class MoveMotion : AnimatedMotion
    {
        private const string moveTriggerString = "Move";
        private const string SkipTriggerString = "Idlle";

        int hashID = 0;

        public MoveMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder, bool specialCheck = false, int hashID = 0) : base(coroutineMono, animator, reproductionOrder, specialCheck)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(moveTriggerString), new AnimationTriggerReproducer(SkipTriggerString));

            if (hashID != 0)
            {
                this.hashID = hashID;
            }
        }

        public override bool CheckCorrectInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }
            return false;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            if (isSpecialCheck == false)
            {
                if (hashID != 0)
                {
                    while (hashID != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
                    {
                        yield return null;
                    }
                    //Debug.Log("hashID " + hashID);
                    //Debug.Log("animator.GetCurrentAnimatorStateInfo(0).fullPathHash " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
                }
                else
                {
                    while (animatorStateInfo.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
                    {
                        //Debug.Log("animatorStateInfo.shortNameHash " + animatorStateInfo.shortNameHash);
                        //Debug.Log("animator.GetCurrentAnimatorStateInfo(0).shortNameHash " + animator.GetCurrentAnimatorStateInfo(0).shortNameHash);
                        yield return null;
                    }
                }


            }
        }

    }

}

