using System.Collections.Generic;


namespace PositionerDemo
{
    public class KimbokoY : Kimboko
    {
        private const UNITTYPE unitType = UNITTYPE.Y;
        private const MOVEDIRECTIONTYPE moveDirectionerType = MOVEDIRECTIONTYPE.DIAGONAL;

        public KimbokoY(int ID, Player ownerPlayer) : base(ID, ownerPlayer, unitType, moveDirectionerType)
        {
            AttackPowerStat attackPow = new AttackPowerStat(4, 4);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(5, 5);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats = new Dictionary<int, Stat>();
            Stats.Add(attackPow.id, attackPow);
            Stats.Add(attackRan.id, attackRan);
            Stats.Add(moveRan.id, moveRan);
            Stats.Add(healthStat.id, healthStat);
            Stats.Add(actionPStat.id, actionPStat);


        }
    }

}
