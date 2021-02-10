using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class SetTriggerAnimationCongifureContainer : ConfigureContainer
    {
        // KimbokoIdlleConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Animator where O : Transform
        public Animator animator { get; set; }
        public string trigger { get; set; }
        public SetTriggerAnimationCongifureContainer(Animator animator, string trigger)
        {
            this.animator = animator;
            this.trigger = trigger;
        }

        public void Execute()
        {
            animator.SetTrigger(trigger);
        }
    }

    #endregion
}