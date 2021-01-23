using System.Collections.Generic;

namespace PositionerDemo
{
    public abstract class Occupier : IOcuppy
    {
        public OCUPPIERTYPE OccupierType { get; protected set; }
        public CARDTARGETTYPE CardTargetType { get; protected set; }
        public Dictionary<ABILITYTYPE, IAbility> Abilities { get; protected set; }
        public Dictionary<STATTYPE, Stat> Stats { get; protected set; }
        public GameObjectAnimatorContainer goAnimContainer { get; protected set; }
        public MOVEDIRECTIONTYPE MoveDirectionerType { get; protected set; }
        public bool IsAlly { get; protected set; }
        public int ID { get; protected set; }
        public int OwnerPlayerID { get; protected set; }
        public Position actualPosition { get; protected set; }
        public virtual IOcuppy GetOcuppy()
        {
            return this;
        }

        public int GetCurrentActionPoints()
        {
            int actionPoints = 0;
            if (Stats.ContainsKey(STATTYPE.ACTIONPOINTS))
            {
                actionPoints = Stats[STATTYPE.ACTIONPOINTS].ActualStatValue;
            }
            return actionPoints;
        }

        public void ResetActionPoints(int amount)
        {
            if (Stats != null)
            {
                if (Stats.ContainsKey(STATTYPE.ACTIONPOINTS))
                {
                    StatModification statModification = new StatModification(this, Stats[STATTYPE.ACTIONPOINTS], amount, STATMODIFIERTYPE.CHANGE, true);
                    Stats[STATTYPE.ACTIONPOINTS].AddStatModifier(statModification);
                    Stats[STATTYPE.ACTIONPOINTS].ApplyModifications();
                    return;
                }
            }
        }

        public void SetGoAnimContainer(GameObjectAnimatorContainer goAnimCon)
        {
            goAnimContainer = goAnimCon;
        }

        public void SetPosition(Position newPosition)
        {
            actualPosition = newPosition;
        }
    }
}
