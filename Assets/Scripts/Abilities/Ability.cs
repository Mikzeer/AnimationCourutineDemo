namespace PositionerDemo
{
    public interface Ability
    {
        // Las ACTION son diferentes habilidades con las cuales puede contar todos los ACTORS del juego
        ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        int ID { get;}
        
        bool OnTryExecute();// Cuando queremos ver si podemos ejecutar esta accion CHEQUEA SI SE TIENE LOS AP NECESARIOS Y CUALQUIER COSA RELATIVA A LA ACTION EN SI        
        void OnStartExecute(); // Cuando empezamos a ejecutar esta accion // EVENTO 1 - OnActionStartExecute
        void Execute(); // Cuando ejecutamos esta accion // Esto es porpio de cada accion        
        void OnEndExecute(); // Cuando terminamos de ejecutar esta accion // EVENTO 2 - OnActionEndExecute
    }

}
