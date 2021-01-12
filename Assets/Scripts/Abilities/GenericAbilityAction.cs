using System;

namespace PositionerDemo
{
    public abstract class GenericAbilityAction<T> : AbilityAction where T : AbilityEventInfo
    {
        public static Action<T> OnActionStartExecute { get; set; }
        public static Action<T> OnActionEndExecute { get; set; }

        public T abilityInfo { get; protected set; }

        public GenericAbilityAction(int ID, IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType) 
                                    : base(ID, performerIOcuppy, actionPointsRequired, abilityType)
        {
        }

        // CON ESTE METODO SETEAMOS LA DATA QUE NECESITEMOS PARA PODER EJECUTAR/CHEQUEAR LA ABILITY ACTION
        public abstract void SetRequireGameData(T gameData);
    }
}