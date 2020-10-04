namespace PositionerDemo
{
    // COMMAND PATTERN PARA LOS MODIFICADORES DE Stats
    public interface IStatModification
    {
        void Execute();
        void Revert();
        STATMODIFICATIONSTATUS status { get; set; }
    }

}
