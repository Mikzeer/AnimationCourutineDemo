using System.Collections;
using System.Collections.Generic;

namespace PositionerDemo
{

    public interface IOcuppy : ICardTarget
    {
        OCUPPIERTYPE OccupierType { get; }
        Dictionary<int,Stat> Stats { get; }
        Dictionary<int, AbilityAction> Abilities { get; }
        Dictionary<ABILITYTYPE, AbilityAction> Abilityes { get; }
        Dictionary<STATTYPE, Stat> Sttats { get; }
        bool IsAlly { get; }
        int GetCurrentActionPoints();
        void ResetActionPoints(int playerID, int amount);
        // ESTE METODO LO VAMOS A DISPARAR CUANDO SELECCIONAMOS AL OCUPPY
        // SI ES UNA UNIDAD//OBJECTO//PLAYER EN EL OnSelect vamos a ver si es nuestra o no
        // SI ES UNA TILE VACIA EN EL OnSelect vamos a chequear que es null el Ocuppier y no vamos a hacer un choto
        void OnSelect(bool isSelected, int playerID); 
    }
}
