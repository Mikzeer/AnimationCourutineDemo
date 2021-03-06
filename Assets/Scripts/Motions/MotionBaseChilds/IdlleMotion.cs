﻿using UnityEngine;
namespace PositionerDemo
{
    public class IdlleMotion : AnimatedMotion
    {
        private const string idlleTriggerString = "Idlle";
        private const string SkipTriggerString = "Idlle";

        public IdlleMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder, bool specialCheck = false) : base(coroutineMono, animator, reproductionOrder, specialCheck)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(idlleTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
        }
    }
}