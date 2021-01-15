using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class ShieldAbilityModifier : AbilityModifier
    {
        private const int SHIELDABILITYMODIFIFERID = 1;
        private const int MODIFEREXECUTIIONORDER = 0;
        public ShieldAbilityModifier(IOcuppy performerIOcuppy) : base(SHIELDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER, performerIOcuppy)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
        }

        public override void Execute(IAbility abilityAction)
        {
            Debug.Log("Enter ShieldAbilityModifier ");
            if (abilityAction.AbilityType == ABILITYTYPE.TAKEDAMAGE)
            {
                //TakeDamageAbility ab = (TakeDamageAbility)abilityAction;
                //if (ab != null)
                //{
                //    // podemos cancelar el takeDamage asi no sigue haciendo nada mas despues de esto
                //    ab.actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
                //    Expire();
                //    //ab.takeDamageAbilityInfo.damageAmount = 0;
                //}
            }           
        }
    }
}
