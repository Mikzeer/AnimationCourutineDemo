using UnityEngine;
using PositionerDemo;
using System.Collections.Generic;

public class TurnManager
{
    private Player actualPlayerTurn;
    private bool playerOneTurn;
    private Player[] players;

    public TurnManager(Player[] players)
    {
        this.players = players;
    }

    public Player GetActualPlayerTurn()
    {
        return actualPlayerTurn;
    }

    public void ChangeTurn(int mngPoints)
    {
        if (actualPlayerTurn == null)
        {
            if (players == null)
            {
                Debug.Log("NO EXISTEN PLAYERS");
                return;
            }
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }

        List<IAbility> unfinishActions = GetUnfinishActions();

        if (unfinishActions.Count > 0)
        {
            GameCreator.Instance.ChangeState(new WaitingState(100, GameCreator.Instance, unfinishActions, GameCreator.Instance.currentState));
        }

        if (playerOneTurn == true)
        {
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }
        else
        {
            actualPlayerTurn = players[0];
            playerOneTurn = true;
        }
    }

    public State ChangeTurnChain(State nextChainState)
    {
        if (actualPlayerTurn == null)
        {
            if (players == null)
            {
                Debug.Log("NO EXISTEN PLAYERS");
                return null;
            }
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }

        List<IAbility> unfinishActions = GetUnfinishActions();

        if (playerOneTurn == true)
        {
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }
        else
        {
            actualPlayerTurn = players[0];
            playerOneTurn = true;
        }

        State nextState;

        if (unfinishActions.Count > 0)
        {
            nextState = new WaitingState(100, GameCreator.Instance, unfinishActions, nextChainState);
        }
        else
        {
            nextState = nextChainState;
        }

        return nextState;
    }

    public State ChangeTurnState(int mngPoints, State posibleNextState)
    {
        if (actualPlayerTurn == null)
        {
            if (players == null)
            {
                Debug.Log("NO EXISTEN PLAYERS");
                return null;
            }
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }

        List<IAbility> unfinishActions = GetUnfinishActions();

        if (playerOneTurn == true)
        {
            actualPlayerTurn = players[1];
            playerOneTurn = false;
        }
        else
        {
            actualPlayerTurn = players[0];
            playerOneTurn = true;
        }

        State nextState; 

        if (unfinishActions.Count > 0)
        {
            nextState = new WaitingState(100, GameCreator.Instance, unfinishActions, posibleNextState);
        }
        else
        {
            nextState = posibleNextState;
        }

        return nextState;
    }

    public List<IAbility> GetUnfinishActions()
    {
        List<IAbility> unfinishActions = new List<IAbility>();

        for (int i = 0; i < actualPlayerTurn.Abilities.Count; i++)
        {
            ABILITYTYPE abType = (ABILITYTYPE)i;
            if (actualPlayerTurn.Abilities[abType].actionStatus == ABILITYEXECUTIONSTATUS.STARTED)
            {
                unfinishActions.Add(actualPlayerTurn.Abilities[abType]);
            }
        }

        return unfinishActions;
    }


}
