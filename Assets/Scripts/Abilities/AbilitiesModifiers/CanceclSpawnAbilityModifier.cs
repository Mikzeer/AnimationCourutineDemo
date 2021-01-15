using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class CanceclSpawnAbilityModifier : AbilityModifier
    {
        private const int ABILITYMODIFIFERID = 2;
        private const int MODIFEREXECUTIIONORDER = 0;
        public CanceclSpawnAbilityModifier(IOcuppy performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER, performerIOcuppy)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
        }

        public override void Execute(IAbility abilityAction)
        {
            Debug.Log("Enter Cancel Spawn ");
            if (abilityAction.AbilityType == ABILITYTYPE.SPAWN)
            {
                SpawnAbility ab = (SpawnAbility)abilityAction;
                if (ab != null)
                {
                    Debug.Log("cencelo el spawn");
                    abilityAction.actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;                    
                }
            }
            Invoker.AddNewCommand(ExpireCmd());
        }
    }
}
