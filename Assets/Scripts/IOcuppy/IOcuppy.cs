using System.Collections.Generic;

namespace PositionerDemo
{

    public interface IOcuppy : ICardTarget
    {
        OCUPPIERTYPE OccupierType { get; }
        Dictionary<STATTYPE, Stat> Stats { get; }
        Dictionary<ABILITYTYPE, IAbility> Abilities { get; }
        MOVEDIRECTIONTYPE MoveDirectionerType { get; }
        bool IsAlly { get; }
        int GetCurrentActionPoints();
        void ResetActionPoints(int amount);
        void OnSelect(bool isSelected, int playerID);
        int OwnerPlayerID { get; }
    }
}
