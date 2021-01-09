using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class CanceclSpawnAbilityModifier : AbilityModifier
    {
        private const int ABILITYMODIFIFERID = 2;
        private const int MODIFEREXECUTIIONORDER = 0;
        //private int spawnAbilityID = 0;
        ABILITYTYPE abilityType = ABILITYTYPE.SPAWN;
        public CanceclSpawnAbilityModifier(IOcuppy performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            //Enter();
        }

        public CanceclSpawnAbilityModifier() : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
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

            Debug.Log("Enter Cancel Spawn ");

            if (abilityAction.AbilityType == ABILITYTYPE.SPAWN)
            {
                SpawnAbility ab = (SpawnAbility)abilityAction;

                if (ab != null)
                {
                    Debug.Log("cencelo el spawn");
                    abilityAction.actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
                    Expire();
                }
            }
        }

        public override ICommand ExecuteCmd(AbilityAction abilityAction)
        {
            IApplyModifierCommand applyModifierCommand = new IApplyModifierCommand(abilityAction, this);
            abilityAction.actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
            return applyModifierCommand;
        }

        public override void Expire()
        {
            performerIOcuppy.Abilities[ABILITYTYPE.SPAWN].RemoveAbilityModifier(this);
        }

    }

}
