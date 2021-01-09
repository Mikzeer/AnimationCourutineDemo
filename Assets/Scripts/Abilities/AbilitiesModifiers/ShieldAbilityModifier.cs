using UnityEngine;

namespace PositionerDemo
{
    public class ShieldAbilityModifier : AbilityModifier
    {
        private const int SHIELDABILITYMODIFIFERID = 1;
        private const int MODIFEREXECUTIIONORDER = 0;
        //private int takeDamageAbilityID = 6;
        ABILITYTYPE abilityType = ABILITYTYPE.TAKEDAMAGE;
        public ShieldAbilityModifier(IOcuppy performerIOcuppy) : base(SHIELDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            Enter();
        }

        public ShieldAbilityModifier() : base(SHIELDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
        }

        public override void Enter()
        {
            if (performerIOcuppy.Abilities.ContainsKey(abilityType))
            {
                performerIOcuppy.Abilities[abilityType].AddAbilityModifier(this);
            }
        }

        public override void Execute(AbilityAction abilityAction)
        {
            base.Execute(abilityAction);

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

        public override void Expire()
        {
            performerIOcuppy.Abilities[abilityType].RemoveAbilityModifier(this);
        }
    }

}
