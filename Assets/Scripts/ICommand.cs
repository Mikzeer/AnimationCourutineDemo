namespace CommandPatternActions
{
    public interface ICommand
    {
        COMMANDEXECUTINSTATE executionState { get; set; }
        bool isRunning { get; set; }
        bool logInsert { get; set; } // Define si lo podemos guardar como un cmd desejecutable
        void Execute();
        void Unexecute();
    }
}