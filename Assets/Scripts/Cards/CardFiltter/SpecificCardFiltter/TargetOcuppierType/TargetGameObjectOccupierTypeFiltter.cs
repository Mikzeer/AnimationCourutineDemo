namespace PositionerDemo
{
    public class TargetGameObjectOccupierTypeFiltter : TargetOcuppierTypeFiltter
    {
        private const OCUPPIERTYPE OC_TYPE = OCUPPIERTYPE.OBJECT;
        private const int FILTTER_ID = 2;
        public TargetGameObjectOccupierTypeFiltter() : base(OC_TYPE, FILTTER_ID)
        {
        }
    }
}