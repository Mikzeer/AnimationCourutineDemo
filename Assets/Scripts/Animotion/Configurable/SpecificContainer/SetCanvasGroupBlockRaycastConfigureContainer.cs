using PositionerDemo;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class SetCanvasGroupBlockRaycastConfigureContainer : ConfigureContainer
    {
        // SetCanvasGroupBlockRaycastConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : CardInGameUINEW where O : Transform
        public CardInGameUINEW cardInGameUI { get; set; }
        public bool isActive { get; set; }
        public SetCanvasGroupBlockRaycastConfigureContainer(CardInGameUINEW cardInGameUI, bool isActive)
        {
            this.cardInGameUI = cardInGameUI;
            this.isActive = isActive;
        }

        public void Execute()
        {
            cardInGameUI.CanvasGroupRaycast(isActive);
        }
    }

    #endregion
}