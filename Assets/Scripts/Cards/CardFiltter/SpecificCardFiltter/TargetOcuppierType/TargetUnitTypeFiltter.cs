namespace PositionerDemo
{
    public class TargetUnitTypeFiltter : TargetUnitOccupierTypeFiltter
    {
        UNITTYPE unitType;
        public TargetUnitTypeFiltter(UNITTYPE unitType) : base()
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