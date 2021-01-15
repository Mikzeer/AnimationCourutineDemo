using UnityEngine;
namespace PositionerDemo
{
    public class AbilityActionStatusConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : IAbility where O : Transform
    {
        public AbilityActionStatusConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.actionStatus = ABILITYEXECUTIONSTATUS.EXECUTED;
        }
    }
}

