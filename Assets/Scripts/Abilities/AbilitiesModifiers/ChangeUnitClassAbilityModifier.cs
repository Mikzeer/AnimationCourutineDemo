using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class ChangeUnitClassAbilityModifier : AbilityModifier
    {
        private const int ABILITYMODIFIFERID = 2;
        private const int MODIFEREXECUTIIONORDER = 1;
        public ChangeUnitClassAbilityModifier(IOcuppy performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER, performerIOcuppy)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
        }

        public override void Execute(IAbility abilityAction)
        {
            Debug.Log("Enter ChangeUnitClassAbilityModifier ");
            if (abilityAction.AbilityType == ABILITYTYPE.SPAWN)
            {
                SpawnAbility ab = (SpawnAbility)abilityAction;
                if (ab != null)
                {
                    Debug.Log("CAMBIO A Y ");
                    ab.actionInfo.spawnUnitType = UNITTYPE.Y;
                }
            }
            Invoker.AddNewCommand(ExpireCmd());
        }
    }
}
