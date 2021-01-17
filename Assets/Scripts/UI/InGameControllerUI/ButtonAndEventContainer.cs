using System;

namespace UIButtonPattern
{
    public class ButtonAndEventContainer
    {
        public ButtonEventFire buttonEventFire;// EL BOTTON EN SI QUE DISPARA EL EVENTO
        public SpecificButtonExecution specificButtonExecution;//EL EVENTO EN SI QUE DISPARA EL BOTTON AL APRETARSE

        public ButtonAndEventContainer(ButtonEventFire buttonEventFire, SpecificButtonExecution specificButtonExecution)
        {
            this.buttonEventFire = buttonEventFire;
            this.specificButtonExecution = specificButtonExecution;

            buttonEventFire.SetEventAction(specificButtonExecution.Execute);
        }

        public ButtonAndEventContainer(ButtonEventFire buttonEventFire, Action specificButtonExecution)
        {
            this.buttonEventFire = buttonEventFire;
            this.specificButtonExecution = null;

            buttonEventFire.SetEventAction(specificButtonExecution);
        }
    }
}
