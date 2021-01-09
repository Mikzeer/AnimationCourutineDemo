using PositionerDemo;
namespace CommandPatternActions
{
    public class IAddAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier abilityModifier;

        public IAddAbilityActionModifierCommand(AbilityAction ability, AbilityModifier abilityModifier)
        {
            this.ability = ability;
            this.abilityModifier = abilityModifier;
        }

        public void Execute()
        {
            isRunning = true;

            ability.AddAbilityModifier(abilityModifier);

            executionState = COMMANDEXECUTINSTATE.FINISH;
            isRunning = false;
        }

        public void Unexecute()
        {
            ability.RemoveAbilityModifier(abilityModifier);
        }
    }
}