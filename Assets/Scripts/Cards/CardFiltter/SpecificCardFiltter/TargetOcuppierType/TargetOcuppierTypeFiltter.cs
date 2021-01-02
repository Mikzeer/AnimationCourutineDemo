namespace PositionerDemo
{
    public abstract class TargetOcuppierTypeFiltter : CardFiltter
    {
        OCUPPIERTYPE ocuppierType;
        public TargetOcuppierTypeFiltter(OCUPPIERTYPE ocuppierType, int ID) : base(ID)
        {
            this.ocuppierType = ocuppierType;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (ocuppier.OccupierType != ocuppierType) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}