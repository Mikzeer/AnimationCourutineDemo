using UnityEngine;

namespace PositionerDemo
{
    public class ChangeUnitClassAbilityModifier : AbilityModifier
    {
        private const int DEFENDABILITYMODIFIFERID = 0;
        private const int MODIFEREXECUTIIONORDER = 0;
        private int spawnAbilityID = 0;
        IOcuppy performerIOcuppy;

        public ChangeUnitClassAbilityModifier(IOcuppy performerIOcuppy) : base(DEFENDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            Enter();
        }

        public override void Enter()
        {
            if (performerIOcuppy.Abilities.ContainsKey(spawnAbilityID))
            {
                performerIOcuppy.Abilities[spawnAbilityID].AddAbilityModifier(this);
            }
        }

        public override void Execute(AbilityAction abilityAction)
        {
            base.Execute(abilityAction);

            Debug.Log("Enter ChangeUnitClassAbilityModifier ");

            if (abilityAction.AbilityType == ABILITYTYPE.SPAWN)
            {
                SpawnAbility ab = (SpawnAbility)abilityAction;

                if (ab != null)
                {
                    Debug.Log("CAMBIO A Y ");
                    ab.spawnAbilityInfo.spawnUnitType = UNITTYPE.Y;
                }
            }
            else
            {
                Expire();
            }


                
            //if (unit.TakeDamageAbility.damage.damageAmountRecived > 1)
            //{
            //    unit.TakeDamageAbility.damage.damageAmountRecived = Mathf.FloorToInt(unit.TakeDamageAbility.damage.damageAmountRecived / 2);
            //}
        }

        public override void Expire()
        {
            performerIOcuppy.Abilities[spawnAbilityID].RemoveAbilityModifier(this);
        }

    }

}
