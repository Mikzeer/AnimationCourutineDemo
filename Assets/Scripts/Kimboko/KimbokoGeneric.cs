using System.Collections.Generic;


namespace PositionerDemo
{
    public class KimbokoGeneric : Kimboko
    {
        public KimbokoGeneric(int ID, Player ownerPlayer, KimbokoPropertiesScriptableObject KimSO) : base(ID, ownerPlayer, KimSO.UnitType, KimSO.MoveDirectionerType)
        {
            AttackPowerStat attackPow = new AttackPowerStat(KimSO.ActualAttackPower, KimSO.MaxAttackPower);
            AttackRangeStat attackRan = new AttackRangeStat(KimSO.ActualAttackRange, KimSO.MaxAttackRange);
            MoveRangeStat moveRan = new MoveRangeStat(KimSO.ActualMoveRange, KimSO.MaxMoveRange);
            HealthStat healthStat = new HealthStat(KimSO.ActualHealth, KimSO.MaxHealth);
            ActionPointStat actionPStat = new ActionPointStat(KimSO.ActualActionPoints, KimSO.MaxActionPoints);

            Stats = new Dictionary<int, Stat>();
            Stats.Add(attackPow.id, attackPow);
            Stats.Add(attackRan.id, attackRan);
            Stats.Add(moveRan.id, moveRan);
            Stats.Add(healthStat.id, healthStat);
            Stats.Add(actionPStat.id, actionPStat);
        }
    }

}
