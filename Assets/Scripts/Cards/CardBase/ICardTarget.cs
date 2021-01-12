using System.Collections.Generic;

namespace PositionerDemo
{
    public interface ICardTarget
    {
        CARDTARGETTYPE CardTargetType { get; }
        IOcuppy GetOcuppy();
        GameObjectAnimatorContainer goAnimContainer { get; }
        void SetGoAnimContainer(GameObjectAnimatorContainer goAnimCon);
    }
   
}
