using UnityEngine;

namespace PositionerDemo
{

    public class DefendAbilityModifier : AbilityModifier
    {
        private const int DEFENDABILITYMODIFIFERID = 0;
        private const int MODIFEREXECUTIIONORDER = 0;
        private int takeDamageAbilityID = 6;
        int duration = 2;

        public DefendAbilityModifier(IOcuppy performerIOcuppy) : base(DEFENDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            Enter();
        }

        public DefendAbilityModifier() : base(DEFENDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
        }


        public override void Enter()
        {
            if (performerIOcuppy.Abilities.ContainsKey(takeDamageAbilityID))
            {
                AnimotionHandler.OnChangeTurn += Expire;
                performerIOcuppy.Abilities[takeDamageAbilityID].AddAbilityModifier(this);
            }
        }

        public override void Execute(AbilityAction abilityAction)
        {
            base.Execute(abilityAction);

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

        public override void Expire()
        {
            duration--;
            if (duration == 0)
            {
                AnimotionHandler.OnChangeTurn -= Expire;
                performerIOcuppy.Abilities[takeDamageAbilityID].RemoveAbilityModifier(this);
            }

        }

    }

}
