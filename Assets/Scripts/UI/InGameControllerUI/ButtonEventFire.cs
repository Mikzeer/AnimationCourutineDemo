using System;

namespace UIButtonPattern
{
    public abstract class ButtonEventFire
    {
        // EL EVENTO DEL BOTTON ESPECIFICO
        public abstract void Suscribe();
        public abstract void Unsuscribe();
        public abstract void Execute();
        public abstract event Action OnButtonPress;

        public void SetEventAction(Action OnButtonPress)
        {
            this.OnButtonPress += OnButtonPress;
        }
    }
}
