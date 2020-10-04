namespace PositionerDemo
{

    public class DefendAbilityModifier : AbilityModifier
    {
        private const int DEFENDABILITYMODIFIFERID = 0;
        private const int MODIFEREXECUTIIONORDER = 0;
        private int takeDamageAbilityID = 4;
        IOcuppy performerIOcuppy;
        int duration = 2;

        public DefendAbilityModifier(IOcuppy performerIOcuppy) : base(DEFENDABILITYMODIFIFERID, MODIFEREXECUTIIONORDER)
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
                AnimotionHandler.OnChangeTurn += Expire;
                performerIOcuppy.Abilities[takeDamageAbilityID].AddAbilityModifier(this);
            }
        }

        public override void Execute(AbilityAction abilityAction)
        {
            base.Execute(abilityAction);
            //if (unit.TakeDamageAbility.damage.damageAmountRecived > 1)
            //{
            //    unit.TakeDamageAbility.damage.damageAmountRecived = Mathf.FloorToInt(unit.TakeDamageAbility.damage.damageAmountRecived / 2);
            //}
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
