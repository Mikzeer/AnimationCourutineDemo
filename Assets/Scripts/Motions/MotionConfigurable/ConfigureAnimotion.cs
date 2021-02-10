namespace PositionerDemo
{
    public abstract class ConfigureAnimotion<T,O> : Configurable
    {
        protected T firstConfigure;
        protected O secondConfigure;
        public int configureOrder { get; private set;}
        public bool isForced { get; private set; }

        public ConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder, bool isForced = false)
        {
            this.firstConfigure = firstConfigure;
            this.secondConfigure = secondConfigure;
            this.configureOrder = configureOrder;
            this.isForced = isForced;
        }

        public ConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = false)
        {
            this.firstConfigure = firstConfigure;
            this.configureOrder = configureOrder;
            this.isForced = isForced;
        }

        public virtual void Configure()
        {
        }
    }
}