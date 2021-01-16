using System.Collections.Generic;
using System.Linq;

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
            MoveRangeStat moveRan = new MoveRangeStat(1, 3);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);
            Stats.Add(moveRan.StatType, moveRan);
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
            Stats.Add(moveRan.StatType, moveRan);
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
            Stats.Add(moveRan.StatType, moveRan);
            return Stats;
        }

        public static Dictionary<STATTYPE, Stat> CreateKimbokoCombineStat(List<Kimboko> combineKimbokos)
        {
            Dictionary<STATTYPE, ActualMax> groupStat = new Dictionary<STATTYPE, ActualMax>();
            // RECORRER LA LISTA DE KIMBOKOS
            for (int i = 0; i < combineKimbokos.Count; i++)
            {
                // RECORRER EL DICCIONARIO DE SUS STATS
                foreach (KeyValuePair<STATTYPE, Stat> stat in combineKimbokos[i].Stats)
                {
                    if (groupStat.ContainsKey(stat.Key))
                    {
                        switch (stat.Key)
                        {
                            case STATTYPE.ATTACKPOW:
                            case STATTYPE.HEALTH:
                                // TANTO ATK POW COMO HP SE SUMAN EL ACTUAL Y EL MAX
                                groupStat[stat.Key].Actual += stat.Value.ActualStatValue;
                                groupStat[stat.Key].Max += stat.Value.MaxStatValue;
                                break;
                            case STATTYPE.ATTACKRANGE:
                            case STATTYPE.MOVERANGE:
                                if (groupStat[stat.Key].Max < stat.Value.MaxStatValue)
                                {
                                    groupStat[stat.Key].Max = stat.Value.MaxStatValue;
                                }
                                if (groupStat[stat.Key].Actual < stat.Value.ActualStatValue)
                                {
                                    groupStat[stat.Key].Actual = stat.Value.ActualStatValue;
                                }
                                break;
                            default:
                                break;
                        }                      
                    }
                    else
                    {
                        groupStat.Add(stat.Key, new ActualMax(stat.Value.ActualStatValue, stat.Value.MaxStatValue));
                    }
                }
            }

            // AGREGO LOS STATS A AL DICCIONARIO
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            if (groupStat.ContainsKey(STATTYPE.ATTACKPOW))
            {
                AttackPowerStat attackPow = new AttackPowerStat(groupStat[STATTYPE.ATTACKPOW].Actual, groupStat[STATTYPE.ATTACKPOW].Max);
                Stats.Add(attackPow.StatType, attackPow);
            }
            if (groupStat.ContainsKey(STATTYPE.ATTACKRANGE))
            {
                AttackRangeStat attackRan = new AttackRangeStat(groupStat[STATTYPE.ATTACKRANGE].Actual, groupStat[STATTYPE.ATTACKRANGE].Max);
                Stats.Add(attackRan.StatType, attackRan);
            }
            if (groupStat.ContainsKey(STATTYPE.MOVERANGE))
            {
                MoveRangeStat moveRan = new MoveRangeStat(groupStat[STATTYPE.MOVERANGE].Actual, groupStat[STATTYPE.MOVERANGE].Max);
                Stats.Add(moveRan.StatType, moveRan);
            }
            if (groupStat.ContainsKey(STATTYPE.HEALTH))
            {
                HealthStat healthStat = new HealthStat(groupStat[STATTYPE.HEALTH].Actual, groupStat[STATTYPE.HEALTH].Max);
                Stats.Add(healthStat.StatType, healthStat);
            }

            // ZX = 116 - ZY = 132 SOLO 1/1
            int points = CombineKimbokoRules.PuntuateKimbokoList(combineKimbokos);
            if (points == 116 || points == 132)
            {
                ActionPointStat actionPStat = new ActionPointStat(1, 1);
                Stats.Add(actionPStat.StatType, actionPStat);
            }
            else
            {
                ActionPointStat actionPStat = new ActionPointStat(2, 2);
                Stats.Add(actionPStat.StatType, actionPStat);
            }      
            return Stats;
        }

        public class ActualMax
        {
            public int Actual;
            public int Max;

            public ActualMax(int Actual, int Max)
            {
                this.Actual = Actual;
                this.Max = Max;
            }
        }

        public static MOVEDIRECTIONTYPE GetMovementDirectionTypeCombineStat(List<Kimboko> combineKimbokos)
        {
            MOVEDIRECTIONTYPE moveType;
            int strong = 0;
            for (int i = 0; i < combineKimbokos.Count; i++)
            {
                int newStrong = CombineKimbokoRules.PuntuateKimboko(combineKimbokos[i]);
                if (newStrong > strong)
                {
                    strong = newStrong;
                }
            }
            if (strong < 17)
            {
                if (combineKimbokos[0].OwnerPlayerID == 0)
                {
                    moveType = MOVEDIRECTIONTYPE.CROSSRIGHT;
                }
                else
                {
                    moveType = MOVEDIRECTIONTYPE.CROSSLEFT;
                }

            }
            else if (strong >= 17 && strong < 115)
            {
                moveType = MOVEDIRECTIONTYPE.DIAGONAL;
            }
            else
            {
                moveType = MOVEDIRECTIONTYPE.MULTI;
            }

            return moveType;
        }

        public static List<Kimboko> GetCombineKimbokoOrder(List<Kimboko> combineKimbokos)
        {
            // CON ESTA FUNCION, ORDENAMOS A LOS KIMBOKOS SEGUN SU RANGO DE UNIDAD Y SU VIDA

            List<Kimboko> characterX = new List<Kimboko>();
            List<Kimboko> characterY = new List<Kimboko>();
            List<Kimboko> characterZ = new List<Kimboko>();

            for (int i = 0; i < combineKimbokos.Count; i++)
            {
                switch (combineKimbokos[i].UnitType)
                {
                    case UNITTYPE.X:
                        characterX.Add(combineKimbokos[i]);
                        break;
                    case UNITTYPE.Y:
                        characterY.Add(combineKimbokos[i]);
                        break;
                    case UNITTYPE.Z:
                        characterZ.Add(combineKimbokos[i]);
                        break;
                    default:
                        break;
                }
            }

            List<Kimboko> finalList = new List<Kimboko>();

            characterZ.OrderBy(o => o.Stats[STATTYPE.HEALTH].ActualStatValue);
            finalList.AddRange(characterZ);
            characterY.OrderBy(o => o.Stats[STATTYPE.HEALTH].ActualStatValue);
            finalList.AddRange(characterY);
            characterX.OrderBy(o => o.Stats[STATTYPE.HEALTH].ActualStatValue);
            finalList.AddRange(characterX);

            return combineKimbokos;
        }
    }
}