using System.Collections.Generic;

namespace PositionerDemo
{

    public interface IOcuppy : ICardTarget
    {
        OCUPPIERTYPE OccupierType { get; }
        Dictionary<STATTYPE, Stat> Stats { get; }
        Dictionary<ABILITYTYPE, IAbility> Abilities { get; }
        MOVEDIRECTIONTYPE MoveDirectionerType { get; }
        Position actualPosition { get; }
        bool IsAlly { get; }
        int GetCurrentActionPoints();
        void ResetActionPoints(int amount);
        int OwnerPlayerID { get; }
        void SetPosition(Position pos);
    }
}
