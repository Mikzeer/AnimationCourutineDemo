namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public interface ConfigurableMotion
    {
        int configureOrder { get; }
        bool isForced { get; }
        void Configure();
    }

    #endregion
}