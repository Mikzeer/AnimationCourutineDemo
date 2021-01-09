using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class ChangeUnitClassAbilityModifier : AbilityModifier
    {
        private const int ABILITYMODIFIFERID = 2;
        private const int MODIFEREXECUTIIONORDER = 1;
        //private int spawnAbilityID = 0;
        ABILITYTYPE abilityType = ABILITYTYPE.SPAWN;
        public ChangeUnitClassAbilityModifier(IOcuppy performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            //Enter();
        }

        public ChangeUnitClassAbilityModifier() : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
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
        }

        public override ICommand ExecuteCmd(AbilityAction abilityAction)
        {
            IApplyModifierCommand applyModifierCommand = new IApplyModifierCommand(abilityAction, this);
            return applyModifierCommand;
        }

        public override void Expire()
        {
            performerIOcuppy.Abilities[ABILITYTYPE.SPAWN].RemoveAbilityModifier(this);
        }
    }
}
