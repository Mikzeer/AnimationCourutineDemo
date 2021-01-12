using PositionerDemo;
namespace CommandPatternActions
{
    public class IRemoveAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier modifier;

        public IRemoveAbilityActionModifierCommand(AbilityAction ability, AbilityModifier modifier)
        {
            this.ability = ability;
            this.modifier = modifier;
        }

        public void Execute()
        {
            ability.abilityModifier.Remove(modifier);
        }

        public void Unexecute()
        {
            ability.abilityModifier.Add(modifier);
        }
    }
}