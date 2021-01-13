using System.Collections.Generic;

namespace PositionerDemo
{
    public static class OccupierStatDatabase
    {
        public static Dictionary<STATTYPE, Stat> CreatePlayerStat()
        {
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(2, 2);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }

        public static Dictionary<STATTYPE, Stat> CreateKimbokoXStat()
        {
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(2, 2);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }

        public static Dictionary<STATTYPE, Stat> CreateKimbokoYStat()
        {
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(4, 4);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(5, 5);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }

        public static Dictionary<STATTYPE, Stat> CreateKimbokoZStat()
        {
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(11, 11);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(11, 11);
            ActionPointStat actionPStat = new ActionPointStat(1, 1);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }

        public static Dictionary<STATTYPE, Stat> CreateKimbokoCombineStat(List<Kimboko> combineKimbokos)
        {
            //TODO
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(11, 11);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            MoveRangeStat moveRan = new MoveRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(11, 11);
            ActionPointStat actionPStat = new ActionPointStat(1, 1);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }
    }
}