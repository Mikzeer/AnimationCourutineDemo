namespace PositionerDemo
{
    public abstract class AbilityModifier
    {
        private int id;
        public int ID { get { return id; } private set { id = value; } }

        protected IOcuppy performerIOcuppy;

        public ABILITYMODIFICATIONSTATUS status;
        public ABILITYMODIFIEREXECUTIONTIME executionTime; // EARLY/END
        public bool executeOnShot; // SE EJECUTA UNA VEZ Y NADA MAS

        // Esto es para saber en que Orden se tiene que ejecutar el Modifier
        // Si un Modifer Cancela una habilidad, entonces lo mejor es ponerlo primero, ya que si al ser CANCEL el status de la habilidad
        // los demas modificadores ni siquiera van a entrar a aplicarse.
        private int modifierExecutionOrder;
        public int ModifierExecutionOrder { get { return modifierExecutionOrder; } private set { modifierExecutionOrder = value; } }

        // Esto es para saber si pueden existir dos o mas modificadores iguales en la misma Ability que se este aplicando.
        // Ejemplo, queremos aplicar el Modifier Escudo en una unidad, pero ya lo tiene aplicado, entonces no tiene sentido que le pongamos
        // Otro modificador igual de Escudo
        private bool canBeRepeat;
        public bool CanBeRepeat { get { return canBeRepeat;} private set { canBeRepeat = value; } }

        // Podriamos tener un bool o un Enum para saber si el Modifier es 
        // NORMAL => SE APLICA Y SE VA DENTRO DE LA MISMA ABILITY O SE APLICA Y LISTO
        // EVENT => REQUIERE SUSCRIBIRSE A ALGUN EVENTO PARA PODER EJECUTARSE
        //          EN ESE CASO PODRIAMOS DESUSCRIBIRNOS SIEMPRE QUE LA UNIT SE MUERA/EVOLUCIONE/COMBINE/DESCOMBINE/FUSIONE
        //          PARA NO TENER UN EVENTO ESCUCHANDO ALGO EN UNA UNIDAD QUE NO EXISTE

        public AbilityModifier(int ID, int ModifierExecutionOrder)
        {
            this.ID = ID;
            this.ModifierExecutionOrder = ModifierExecutionOrder;
        }

        public void SetOccupier(IOcuppy occupier)
        {
            this.performerIOcuppy = occupier;
        }

        public virtual void Enter()
        {

        }

        public virtual void Expire()
        {

        }

        public virtual void Execute(AbilityAction abilityAction)
        {
            if (executeOnShot)
            {
                if (status == ABILITYMODIFICATIONSTATUS.ExecuteSucceeded)
                {
                    return;
                }
            }
        }

    }

}
