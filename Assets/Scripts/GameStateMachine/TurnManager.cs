﻿using UnityEngine;
using PositionerDemo;
using System;
using System.Collections;
using System.Collections.Generic;
public class TurnManager
{
    public static event Action OnChangeTurn;
    public static event Action<int, int> OnUnitResetActionPoints;
    public static event Action<int, int> OnPlayerResetActionPoints;

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

        List<AbilityAction> unfinishActions = GetUnfinishActions();

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

        OnChangeTurn?.Invoke();
        OnUnitResetActionPoints?.Invoke(actualPlayerTurn.PlayerID, 2);
        OnPlayerResetActionPoints?.Invoke(actualPlayerTurn.PlayerID, mngPoints);
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

        List<AbilityAction> unfinishActions = GetUnfinishActions();

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

        List<AbilityAction> unfinishActions = GetUnfinishActions();

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

        OnChangeTurn?.Invoke();
        OnUnitResetActionPoints?.Invoke(actualPlayerTurn.PlayerID, 2);
        OnPlayerResetActionPoints?.Invoke(actualPlayerTurn.PlayerID, mngPoints);

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

    public List<AbilityAction> GetUnfinishActions()
    {
        List<AbilityAction> unfinishActions = new List<AbilityAction>();

        for (int i = 0; i < actualPlayerTurn.Abilities.Count; i++)
        {
            if (actualPlayerTurn.Abilities[i].actionStatus == ABILITYEXECUTIONSTATUS.STARTED)
            {
                unfinishActions.Add(actualPlayerTurn.Abilities[i]);
            }
        }

        return unfinishActions;
    }

}  