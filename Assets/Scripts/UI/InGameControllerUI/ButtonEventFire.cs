using System;

namespace UIButtonPattern
{
    public abstract class ButtonEventFire
    {
        // EL EVENTO DEL BOTTON ESPECIFICO
        public abstract void Suscribe(Action toSub);
        public abstract void Unsuscribe(Action toSub);
        public abstract void Execute();
        public abstract event Action OnButtonPress;

        public void SetEventAction(Action OnButtonPress)
        {
            this.OnButtonPress += OnButtonPress;
        }
    }
}
