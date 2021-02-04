namespace PositionerDemo
{
    public class TargetPlayerOccupierTypeFiltter : TargetOcuppierTypeFiltter
    {
        private const OCUPPIERTYPE OC_TYPE = OCUPPIERTYPE.PLAYER;
        private const int FILTTER_ID = 1;
        public TargetPlayerOccupierTypeFiltter() : base(OC_TYPE, FILTTER_ID)
        {
        }
    }
}