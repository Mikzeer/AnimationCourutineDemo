namespace PositionerDemo
{
    public abstract class ConfigureAnimotion<T,O> : Configurable
    {
        protected T firstConfigure;
        protected O secondConfigure;

        public int configureOrder { get; private set;}

        public ConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder)
        {
            this.firstConfigure = firstConfigure;
            this.secondConfigure = secondConfigure;
            this.configureOrder = configureOrder;
        }

        public ConfigureAnimotion(T firstConfigure, int configureOrder)
        {
            this.firstConfigure = firstConfigure;
            this.configureOrder = configureOrder;
        }

        public virtual void Configure()
        {

        }

    }

}

