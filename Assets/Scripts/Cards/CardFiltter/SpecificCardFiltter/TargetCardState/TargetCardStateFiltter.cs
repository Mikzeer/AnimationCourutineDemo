namespace PositionerDemo
{
    public abstract class TargetCardStateFiltter : CardFiltter
    {
        protected IResultData cardDataToCheck;
        protected IResultData rdToCheckAgainst;
        protected COMPARATIONTYPE comparationType;
        public TargetCardStateFiltter(COMPARATIONTYPE comparationType, int ID) : base(ID)
        {
            this.comparationType = comparationType;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;
            IOcuppy occupier = cardTarget.GetOcuppy();
            if (occupier == null) return null;
            Player player = (Player)cardTarget;
            if (player == null) return null;            
            return base.CheckTarget(cardTarget);
        }
    }
}