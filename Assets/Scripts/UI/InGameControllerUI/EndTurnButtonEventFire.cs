using System;

namespace UIButtonPattern
{
    public class EndTurnButtonEventFire : ButtonEventFire
    {
        public override event Action OnButtonPress;

        public override void Execute()
        {
            OnButtonPress?.Invoke();
        }

        public override void Suscribe()
        {
            UIController.OnEndTurnActionClicked += Execute;
        }

        public override void Unsuscribe()
        {
            UIController.OnEndTurnActionClicked -= Execute;
        }
    }
}
