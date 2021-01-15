using PositionerDemo;

namespace CommandPatternActions
{
    public class IAddAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        IAbility ability;
        AbilityModifier abilityModifier;

        public IAddAbilityActionModifierCommand(IAbility ability, AbilityModifier abilityModifier)
        {
            this.ability = ability;
            this.abilityModifier = abilityModifier;
            logInsert = true;
        }

        public void Execute()
        {
            ability.abilityModifier.Add(abilityModifier);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            Invoker.AddNewCommand(new IRemoveAbilityActionModifierCommand(ability, abilityModifier));
            Invoker.ExecuteCommands();
            //ability.RemoveAbilityModifier(abilityModifier);
        }
    }
}