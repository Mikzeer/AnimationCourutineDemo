using PositionerDemo;
using System.Collections.Generic;

namespace CommandPatternActions
{
    public class IAddSimpleAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        List<AbilityModifier> modifierList;
        AbilityModifier abilityModifier;

        public IAddSimpleAbilityActionModifierCommand(List<AbilityModifier> modifierList, AbilityModifier abilityModifier)
        {
            this.modifierList = modifierList;
            this.abilityModifier = abilityModifier;
            logInsert = true;
        }

        public void Execute()
        {
            modifierList.Add(abilityModifier);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            modifierList.Remove(abilityModifier);
        }
    }
}