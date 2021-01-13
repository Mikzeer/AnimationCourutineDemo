using PositionerDemo;
namespace CommandPatternActions
{
    public class IAddAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier abilityModifier;

        public IAddAbilityActionModifierCommand(AbilityAction ability, AbilityModifier abilityModifier)
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
            ability.RemoveAbilityModifier(abilityModifier);
        }
    }
}