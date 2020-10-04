namespace PositionerDemo
{
    public abstract class CommonAbility : Ability
    {
        // Esto lo va a tener cada clase en si... 
        //event Action<ActionEventInformation> OnActionStartExecute { get; set; }
        //event Action<ActionEventInformation> OnActionEndExecute { get; set; }

        private int id;
        public int ID { get { return id; } private set { id = value; } }
        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        public abstract bool OnTryExecute();
        public abstract void OnStartExecute();
        public abstract void Execute();
        public abstract void OnEndExecute();
        public void Perform()
        {
            if (OnTryExecute())
            {
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                OnStartExecute();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                Execute();
                OnEndExecute();
            }       
        }
    }

}
