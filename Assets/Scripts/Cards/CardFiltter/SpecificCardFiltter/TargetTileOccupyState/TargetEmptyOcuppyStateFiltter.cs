namespace PositionerDemo
{
    public class TargetEmptyOcuppyStateFiltter : TargetOcuppyStateFiltter
    {
        private const bool IS_OCUPPIED = false;
        private const int FILTTER_ID = 39;
        public TargetEmptyOcuppyStateFiltter() : base(IS_OCUPPIED, FILTTER_ID)
        {
        }
    }
}