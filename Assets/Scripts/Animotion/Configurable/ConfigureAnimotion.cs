namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class ConfigureAnimotion : ConfigurableMotion
    {
        ConfigureContainer configureContainer;
        public int configureOrder { get; private set; }
        public bool isForced { get; private set; }

        public ConfigureAnimotion(ConfigureContainer configureContainer, int configureOrder, bool isForced = false)
        {
            this.configureContainer = configureContainer;
            this.configureOrder = configureOrder;
            this.isForced = isForced;
        }

        public void Configure()
        {
            configureContainer.Execute();
        }
    }

    #endregion
}