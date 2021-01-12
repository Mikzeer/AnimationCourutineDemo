namespace PositionerDemo
{
    public interface Ability
    {
        ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        int ID { get;}        
        bool OnTryExecute();
        void OnStartExecute();
        void Execute(); 
        void OnEndExecute(); 
    }
}
