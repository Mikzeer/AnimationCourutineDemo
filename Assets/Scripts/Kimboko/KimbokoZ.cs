using System.Collections.Generic;


namespace PositionerDemo
{
    public class KimbokoZ : Kimboko
    {
        private const UNITTYPE unitType = UNITTYPE.Z;
        private const MOVEDIRECTIONTYPE moveDirectionerType = MOVEDIRECTIONTYPE.MULTI;

        public KimbokoZ(int ID, Player ownerPlayer) : base(ID, ownerPlayer, unitType, moveDirectionerType)
        {
            Stats = OccupierStatDatabase.CreateKimbokoZStat();
            //AttackPowerStat attackPow = new AttackPowerStat(11, 11);
            //AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            //MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            //HealthStat healthStat = new HealthStat(11, 11);
            //ActionPointStat actionPStat = new ActionPointStat(1, 1);
            //Stats = new Dictionary<int, Stat>();
            //Stats.Add(attackPow.id, attackPow);
            //Stats.Add(attackRan.id, attackRan);
            //Stats.Add(moveRan.id, moveRan);
            //Stats.Add(healthStat.id, healthStat);
            //Stats.Add(actionPStat.id, actionPStat);
        }
    }

}
