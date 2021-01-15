using CommandPatternActions;

namespace PositionerDemo
{
    public abstract class AbilityModifier
    {
        public int ID { get; private set; }
        public int ModifierExecutionOrder { get; private set; }
        protected IOcuppy performerIOcuppy;
        // SI ES UN MODIFIER QUE SE EJECUTA AL INICIO ENTONCES ES UN EARLYMODIFICATION, SU APLICACION SE HACE ANTES DE REALIZAR LA ABILITYACTION
        // SI ES UN MODIFIER QUE SE EJECUTA AL FINAL ENTONCES ES UN ENDMODIFICATION, SU APLICACION SE HACE DESPUES DE REALIZAR LA ABILITYACTION        
        public ABILITYMODIFIEREXECUTIONTIME executionTime { get; protected set; }// EARLY/END

        public AbilityModifier(int ID, int ModifierExecutionOrder, IOcuppy performerIOcuppy)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.ID = ID;
            this.ModifierExecutionOrder = ModifierExecutionOrder;
        }

        public virtual void Execute(IAbility abilityAction)
        {
        }

        public ICommand ExecuteCmd(IAbility abilityAction)
        {
            IApplyModifierCommand applyModifierCommand = new IApplyModifierCommand(abilityAction, this);
            return applyModifierCommand;
        }

        public ICommand ExpireCmd()
        {
            IRemoveAbilityActionModifierCommand remove = new IRemoveAbilityActionModifierCommand(performerIOcuppy.Abilities[ABILITYTYPE.SPAWN], this);
            return remove;
        }

    }

}
