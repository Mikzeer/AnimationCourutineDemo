using UnityEngine;

namespace PositionerDemo
{
    public class ChangeUnitClassAbilityModifier : AbilityModifier
    {
        private const int ABILITYMODIFIFERID = 2;
        private const int MODIFEREXECUTIIONORDER = 0;
        private int spawnAbilityID = 0;

        public ChangeUnitClassAbilityModifier(IOcuppy performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            Enter();
        }

        public ChangeUnitClassAbilityModifier() : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
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
        }

        public override void Expire()
        {
            performerIOcuppy.Abilities[spawnAbilityID].RemoveAbilityModifier(this);
        }

    }

}
