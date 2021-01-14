using System;

namespace PositionerDemo
{
    public abstract class GenericAbilityAction<T> : AbilityAction
    {
        public static Action<T> OnActionStartExecute { get; set; }
        public static Action<T> OnActionEndExecute { get; set; }

        public T actionInfo { get; protected set; }

        public GenericAbilityAction(IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType) 
                                    : base(performerIOcuppy, actionPointsRequired, abilityType)
        {
        }

        // CON ESTE METODO SETEAMOS LA DATA QUE NECESITEMOS PARA PODER EJECUTAR/CHEQUEAR LA ABILITY ACTION
        public abstract void SetRequireGameData(T gameData);

        public override void OnStartExecute()
        {
            OnActionStartExecute?.Invoke(actionInfo);
        }

        public override void OnEndExecute()
        {
            OnActionEndExecute?.Invoke(actionInfo);
        }
    }
}