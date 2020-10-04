namespace PositionerDemo
{
    public abstract class AbilityModifier
    {
        private int id;
        public int ID { get { return id; } private set { id = value; } }

        public ABILITYMODIFICATIONSTATUS status;
        public ABILITYMODIFIEREXECUTIONTIME executionTime;
        public bool executeOnShot; // SE EJECUTA UNA VEZ Y NADA MAS

        private int modifierExecutionOrder;
        public int ModifierExecutionOrder { get { return modifierExecutionOrder; } private set { modifierExecutionOrder = value; } }

        public AbilityModifier(int ID, int ModifierExecutionOrder)
        {
            this.ID = ID;
            this.ModifierExecutionOrder = ModifierExecutionOrder;
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
