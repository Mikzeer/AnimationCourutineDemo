namespace MikzeerGame
{
    namespace UI
    {
        public interface SpecificAbilityExecution
        {
            // LO QUE PASA AL EJECUTAR ESE EVENTO ESPECIFICO DE BOTON
            void Execute();
            string name { get; }
        }

    }
}