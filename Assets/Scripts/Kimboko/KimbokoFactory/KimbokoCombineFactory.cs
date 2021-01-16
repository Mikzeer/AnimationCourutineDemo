using System.Collections.Generic;

namespace PositionerDemo
{
    public class KimbokoCombineFactory : KimbokoFactory
    {
        List<Kimboko> kimbokos;
        MOVEDIRECTIONTYPE directionType;
        public KimbokoCombineFactory(List<Kimboko> kimbokos)
        {
            this.kimbokos = kimbokos;
            this.directionType = OccupierStatDatabase.GetMovementDirectionTypeCombineStat(kimbokos);
        }

        public override Kimboko CreateKimboko(int ID, Player player)
        {
            return new KimbokoCombine(kimbokos, ID, player, directionType);
        }
    }

}
