namespace PositionerDemo
{
    public class KimbokoY : Kimboko
    {
        private const UNITTYPE unitType = UNITTYPE.Y;
        private const MOVEDIRECTIONTYPE moveDirectionerType = MOVEDIRECTIONTYPE.DIAGONAL;

        public KimbokoY(int ID, Player ownerPlayer) : base(ID, ownerPlayer, unitType, moveDirectionerType)
        {
            Stats = OccupierStatDatabase.CreateKimbokoYStat();
        }
    }
}
