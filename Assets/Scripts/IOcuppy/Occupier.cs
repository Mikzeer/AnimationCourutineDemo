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
        public bool IsAlly { get; protected set; }
        public int ID { get; protected set; }
        public int OwnerPlayerID { get; protected set; }

        public virtual IOcuppy GetOcuppy()
        {
            return this;
        }
        /// <summary>
        ///  Se Dispara al seleccionar un Occupier de alguna manera para mostrar alguna informacion especifica de el.
        /// </summary>
        /// <param name="isSelected"></param>
        /// <param name="playerID"></param>
        public virtual void OnSelect(bool isSelected, int playerID)
        {            
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
                    StatModification statModification = new StatModification(this, Stats[STATTYPE.ACTIONPOINTS], amount, STATMODIFIERTYPE.CHANGE);
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
    }
}
