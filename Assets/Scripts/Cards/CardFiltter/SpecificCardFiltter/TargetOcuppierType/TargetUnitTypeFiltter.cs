namespace PositionerDemo
{
    public class TargetUnitTypeFiltter : TargetOcuppierTypeFiltter
    {
        UNITTYPE unitType;
        private const OCUPPIERTYPE OC_TYPE = OCUPPIERTYPE.UNIT;
        private const int FILTTER_ID = 45;
        public TargetUnitTypeFiltter(UNITTYPE unitType) : base(OC_TYPE, FILTTER_ID)
        {
            this.unitType = unitType;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (base.CheckTarget(cardTarget) == null) return null;

            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            Kimboko kim = (Kimboko)ocuppier;
            if (kim == null) return null;
            if (kim.UnitType != unitType) return null;

            return cardTarget;
        }
    }
}