using System;
using System.Collections.Generic;
using System.Linq;

namespace PositionerDemo
{
    public abstract class Ability<T> : IAbility
    {
        public List<AbilityModifier> abilityModifier { get; private set; }
        public ABILITYTYPE AbilityType { get; private set; }
        public IOcuppy performerIOcuppy { get; private set; }
        private int actionPointsRequired; 
        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }

        public static Action<T> OnActionStartExecute { get; set; }
        public static Action<T> OnActionEndExecute { get; set; }
        public T actionInfo { get; protected set; }

        public int GetActionPointsRequiredToUseAbility() => actionPointsRequired;

        public Ability(IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE AbilityType)
        {
            abilityModifier = new List<AbilityModifier>();
            this.performerIOcuppy = performerIOcuppy;
            this.actionPointsRequired = actionPointsRequired;
            this.AbilityType = AbilityType;
        }

        // Cuando queremos ver si podemos ejecutar esta accion CHEQUEA SI SE TIENE LOS AP NECESARIOS Y CUALQUIER COSA RELATIVA A LA ACTION EN SI
        public abstract bool CanIExecute();

        // CON ESTE METODO SETEAMOS LA DATA QUE NECESITEMOS PARA PODER EJECUTAR/CHEQUEAR LA ABILITY ACTION
        public abstract void SetRequireGameData(T gameData);

        public void OnStartExecute()
        {
            OnActionStartExecute?.Invoke(actionInfo);
        }

        public void OnEndExecute()
        {
            OnActionEndExecute?.Invoke(actionInfo);
        }

        public bool IsModifierApply(int modifierID)
        {
            if (abilityModifier.Count == 0) return false;
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].ID == modifierID) return true;
            }
            return false;
        }

        public List<AbilityModifier> GetAbilityModifierOrderByExecution()
        {
            abilityModifier = abilityModifier.OrderBy(c => c.ModifierExecutionOrder).ToList();
            return abilityModifier;
        }
    }
}