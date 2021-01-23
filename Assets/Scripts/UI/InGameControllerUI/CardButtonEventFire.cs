using System;

namespace UIButtonPattern
{
    public class CardButtonEventFire : ButtonEventFire
    {
        public override event Action OnButtonPress;

        public override void Execute()
        {
            OnButtonPress?.Invoke();
        }

        public override void Suscribe(Action toSub)
        {
            toSub += Execute;
        }

        public override void Unsuscribe(Action toSub)
        {
            toSub -= Execute;
        }
    }
}
