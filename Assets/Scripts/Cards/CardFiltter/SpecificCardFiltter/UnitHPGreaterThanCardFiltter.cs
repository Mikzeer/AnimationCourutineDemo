namespace PositionerDemo
{
    public class UnitHPGreaterThanCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 10;
        private const int HEALTHSTATID = 0;
        private int hpValueToEvaluate = 2;

        public UnitHPGreaterThanCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.UNIT)
            {
                return null;
            }

            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (kimboko.Stats.ContainsKey(HEALTHSTATID))
                {
                    if (kimboko.Stats[HEALTHSTATID].ActualStatValue > hpValueToEvaluate)
                    {
                        return cardTarget;
                    }
                    
                }
            }
            else
            {
                return null;
            }
            return null;
        }
    }
}


