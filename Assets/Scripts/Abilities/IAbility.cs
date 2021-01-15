using System.Collections.Generic;

namespace PositionerDemo
{
    public interface IAbility
    {
        ABILITYTYPE AbilityType { get; }
        IOcuppy performerIOcuppy { get; }
        ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        List<AbilityModifier> abilityModifier { get; }
        bool CanIExecute();
        void OnStartExecute();
        void OnEndExecute();
        int GetActionPointsRequiredToUseAbility();
        bool IsModifierApply(int modifierID);
        List<AbilityModifier> GetAbilityModifierOrderByExecution();
    }
}