namespace PositionerDemo
{
    public class ShieldAbilityModifier : AbilityModifier
    {
        private const int SHIELDABILITYMODIFIFERID = 1;
        private const int MODIFEREXECUTIIONORDER = 0;
        private int takeDamageAbilityID = 4;
        IOcuppy performerIOcuppy;

        public ShieldAbilityModifier(IOcuppy performerIOcuppy) : base(SHIELDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
        {
            this.performerIOcuppy = performerIOcuppy;
            executionTime = ABILITYMODIFIEREXECUTIONTIME.EARLY;
            executeOnShot = false;
            Enter();
        }

        public override void Enter()
        {
            if (performerIOcuppy.Abilities.ContainsKey(takeDamageAbilityID))
            {
                performerIOcuppy.Abilities[takeDamageAbilityID].AddAbilityModifier(this);
            }
        }

        public override void Execute(AbilityAction abilityAction)
        {
            base.Execute(abilityAction);
            //unit.TakeDamageAbility.damage.damageAmountRecived = 0;
            // podemos cancelar el takeDamage asi no sigue haciendo nada mas despues de esto
            Expire();
        }

        public override void Expire()
        {
            performerIOcuppy.Abilities[takeDamageAbilityID].RemoveAbilityModifier(this);
        }
    }

}
