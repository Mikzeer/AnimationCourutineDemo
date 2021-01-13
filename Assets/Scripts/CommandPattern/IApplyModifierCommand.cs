using PositionerDemo;
namespace CommandPatternActions
{
    public class IApplyModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier abilityModifier;

        public IApplyModifierCommand(AbilityAction ability, AbilityModifier abilityModifier)
        {
            this.ability = ability;
            this.abilityModifier = abilityModifier;
        }

        public void Execute()
        {
            abilityModifier.Execute(ability);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            //abilityModifier.Unexecute(ability);
        }
    }
}