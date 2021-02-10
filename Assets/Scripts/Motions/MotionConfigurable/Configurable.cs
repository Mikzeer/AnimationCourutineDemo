namespace PositionerDemo
{
    public interface Configurable
    {
        int configureOrder { get; }
        bool isForced { get; }
        void Configure();
    }
}