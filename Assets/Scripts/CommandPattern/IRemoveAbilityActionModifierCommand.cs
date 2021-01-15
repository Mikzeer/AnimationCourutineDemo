using PositionerDemo;
namespace CommandPatternActions
{
    public class IRemoveAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        IAbility ability;
        AbilityModifier modifier;

        public IRemoveAbilityActionModifierCommand(IAbility ability, AbilityModifier modifier)
        {
            this.ability = ability;
            this.modifier = modifier;
            logInsert = true;
        }

        public void Execute()
        {
            ability.abilityModifier.Remove(modifier);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            ability.abilityModifier.Add(modifier);

        }
    }
}