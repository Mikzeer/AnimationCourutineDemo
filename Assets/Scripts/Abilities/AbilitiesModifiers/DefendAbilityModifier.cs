using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class DefendAbilityModifier : TimeEventAbilityModifier
    {
        private const int DEFENDABILITYMODIFIFERID = 0;
        private const int MODIFEREXECUTIIONORDER = 0;
        int duration = 2;
        public DefendAbilityModifier(IOcuppy performerIOcuppy) : base(DEFENDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER, performerIOcuppy)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
        }

        public override void Enter()
        {
            TurnController.OnChangeTurn += RestDuration;
        }

        public override void Execute(IAbility abilityAction)
        {
            Debug.Log("Enter DefendAbilityModifier ");
            if (abilityAction.AbilityType == ABILITYTYPE.TAKEDAMAGE)
            {
                //TakeDamageAbility ab = (TakeDamageAbility)abilityAction;
                //if (ab != null)
                //{
                //    if (ab.takeDamageAbilityInfo.damageAmount > 1)
                //    {
                //        ab.takeDamageAbilityInfo.damageAmount = Mathf.FloorToInt(ab.takeDamageAbilityInfo.damageAmount / 2);
                //    }
                //}
            }
        }

        public override void RestDuration()
        {
            duration--;
            if (duration == 0)
            {
                TurnController.OnChangeTurn -= RestDuration;
                Invoker.AddNewCommand(ExpireCmd());
            }
        }
    }
}
