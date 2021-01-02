namespace PositionerDemo
{
    public abstract class TargetStatAmountFiltter : CardFiltter
    {
        COMPARATIONTYPE comparationType;
        protected StatIResultData rDToCheck; // UNIT STAT TO CHECK
        protected StatIResultData rDToCheckAgainst; // result data to check against

        public TargetStatAmountFiltter(COMPARATIONTYPE comparationType, int ID) : base(ID)
        {
            this.comparationType = comparationType;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.UNIT || cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO || cardTarget.CardTargetType != CARDTARGETTYPE.BOARDOBJECT)
            {
                return null;
            }
            IOcuppy occupier = cardTarget.GetOcuppy();
            if (occupier == null) return null;
            rDToCheck.SetOcuppier(occupier);
            rDToCheckAgainst.SetOcuppier(occupier);
            ResultDataValidator validator = new ResultDataValidator(comparationType, rDToCheck, rDToCheckAgainst);
            if (validator.IsValid() == false ) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}