using System.Collections.Generic;

public abstract class SelectionState<T> : SubState
{
    List<T> posibleSelectionTargets;

    public SelectionState(State previousState, GameCreator gsMachine) : base(previousState, gsMachine)
    {
    }

    public virtual void SetTargets(List<T> posibleTargets)
    {
        posibleSelectionTargets = posibleTargets;
        // MOVE => Pinto Las Tiles
        // ATTACK => Selecciono las tiles a atacar y marco a los enemigos como van a quedar de vida ?
        // SPAWN => Pinto las tiles y marco a las unidades combinables
    }

    public virtual void SetSelection(T selection)
    {
        // MOVE/ATTACK/COMBINE/SPAWN Si esta dentro de las posibles Ejecuto, sino Cancelo
        // DECOMBINE Si esta dentro de las posibles y no esta esa tile y la lista cumplio con lo requerido Ejecuto, sino Cancelo
        // USE CARD // SELECT THE CARD TARGETS
    }
}










