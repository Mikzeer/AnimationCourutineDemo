namespace PositionerDemo
{
    public abstract class TimeEventAbilityModifier : AbilityModifier
    {
        // PARA LOS MODIFIERS QUE DURAN X CANTIDAD DE TURNO, O CUANDO PASA X COSA DOS ATAQUES DIRECTOS
        // MOVERSE DOS VECES, O TODO MODIFIER QUE NO SE VAYA UNA VEZ UTILIZADO Y QUE TENGA UNA DETERMINADA DURACION

        public TimeEventAbilityModifier(int ID, int ModifierExecutionOrder, IOcuppy performerIOcuppy) : base(ID, ModifierExecutionOrder, performerIOcuppy)
        {
            Enter();
        }

        public virtual void Enter()
        {

        }

        public virtual void RestDuration()
        {

        }
    }

}
