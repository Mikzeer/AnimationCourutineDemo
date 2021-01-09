using System.Collections.Generic;


namespace PositionerDemo
{
    public class KimbokoX : Kimboko
    {
        private const UNITTYPE unitType = UNITTYPE.X;
        private const MOVEDIRECTIONTYPE moveDirectionerType = MOVEDIRECTIONTYPE.CROSSLEFT;

        public KimbokoX(int ID, Player ownerPlayer) : base(ID, ownerPlayer, unitType, moveDirectionerType)
        {
            Stats = OccupierStatDatabase.CreateKimbokoXStat();
            //AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            //AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            //MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            //HealthStat healthStat = new HealthStat(2, 2);
            //ActionPointStat actionPStat = new ActionPointStat(2, 2);
            //Stats = new Dictionary<int, Stat>();
            //Stats.Add(attackPow.id, attackPow);
            //Stats.Add(attackRan.id, attackRan);
            //Stats.Add(moveRan.id, moveRan);
            //Stats.Add(healthStat.id, healthStat);
            //Stats.Add(actionPStat.id, actionPStat);
        }
    }

}
