using System.Collections;
using System.Collections.Generic;

namespace PositionerDemo
{

    public interface IOcuppy : ICardTarget
    {
        OCUPPIERTYPE OccupierType { get; }
        void OnSelect(bool isSelected, int playerID); 
        Dictionary<int,Stat> Stats { get; }
        Dictionary<int, AbilityAction> Abilities { get; }

        int GetCurrentActionPoints();
        void ResetActionPoints(int playerID);
        // ESTE METODO LO VAMOS A DISPARAR CUANDO SELECCIONAMOS AL OCUPPY
        // SI ES UNA UNIDAD//OBJECTO//PLAYER EN EL OnSelect vamos a ver si es nuestra o no
        // SI ES UNA TILE VACIA EN EL OnSelect vamos a chequear que es null el Ocuppier y no vamos a hacer un choto

        // Todos los IOcuppy UNIT / OBJECT / PLAYER van a tener una diccionario de Actions
        // Todos los IOcuppy UNIT / OBJECT / PLAYER van a tener una diccionario de Stats
        // Podrian llegar a tener los Stats como las Actions en la Clase en si, y para 
        // buscarlos solo buscamos el IOcuppy.MoveAction.Perform() // IOcuppy.MoveRange
        // if IOcuppy.Actions.Contains(actionID) // if IOcuppy.Stats.Contains(statID)
        // o tambien podra ser IOcuppy.Actions[actionID].Perform() // IOcuppy.Stats[statID]

        // IOccupy ? ActionAbility.Count > 0 
        // if ActionAbility.Contains(int AbilityID);
        // IOccupy.ActionAbility(AbilityID).Execute();
    }

}
