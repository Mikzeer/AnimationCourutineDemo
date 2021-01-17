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

        public override void Suscribe()
        {
            UIController.OnTakeCardActionClicked += Execute;
        }

        public override void Unsuscribe()
        {
            UIController.OnTakeCardActionClicked -= Execute;
        }
    }
}
