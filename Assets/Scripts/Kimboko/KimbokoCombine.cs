using System.Collections.Generic;


namespace PositionerDemo
{
    public class KimbokoCombine : Kimboko
    {
        private const UNITTYPE unitType = UNITTYPE.COMBINE;
        public List<Kimboko> kimbokos { get; private set; }
        public KimbokoCombine(List<Kimboko> kimbokos, int ID, Player ownerPlayer, MOVEDIRECTIONTYPE moveDirectionerType) 
                       : base(ID, ownerPlayer, unitType, moveDirectionerType)
        {
            this.kimbokos = kimbokos;
            Stats = OccupierStatDatabase.CreateKimbokoCombineStat(kimbokos);
        }
    }

}
