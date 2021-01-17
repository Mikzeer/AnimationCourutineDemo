using System.Collections.Generic;

namespace UIButtonPattern
{
    public class UIInputButtonPattern
    {
        List<ButtonAndEventContainer> buttonPartners;

        public UIInputButtonPattern(List<ButtonAndEventContainer> buttonPartners)
        {
            this.buttonPartners = buttonPartners;
        }

        public void Suscribe()
        {
            for (int i = 0; i < buttonPartners.Count; i++)
            {
                buttonPartners[i].buttonEventFire.Suscribe();
            }
        }

        public void Unsuscribe()
        {
            for (int i = 0; i < buttonPartners.Count; i++)
            {
                buttonPartners[i].buttonEventFire.Unsuscribe();
            }
        }

    }
}
