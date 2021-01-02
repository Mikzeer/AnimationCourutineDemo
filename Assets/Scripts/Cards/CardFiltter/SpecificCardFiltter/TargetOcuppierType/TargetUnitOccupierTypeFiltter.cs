namespace PositionerDemo
{
    public class TargetUnitOccupierTypeFiltter : TargetOcuppierTypeFiltter
    {
        private const OCUPPIERTYPE OC_TYPE = OCUPPIERTYPE.UNIT;
        private const int FILTTER_ID = 0;
        public TargetUnitOccupierTypeFiltter() : base(OC_TYPE, FILTTER_ID)
        {
        }
    }
}