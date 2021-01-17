using UnityEngine;
using PositionerDemo;
using System.Collections.Generic;
using System;

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

public class TurnController
{
    public Player[] Players { get; private set; }
    public int TurnCount { get; private set; }
    public Player StarterPlayer { get; private set; }
    public Player CurrentPlayerTurn { get; private set; }
    public Player NextPlayerTurn { get; private set; }
    public static event Action OnChangeTurn;

    public TurnController(Player[] Players)
    {
        this.Players = Players;
        TurnCount = -4; // 4 fases de administracion
    }

    public void DecideStarterPlayer()
    {
        var randomIndex = UnityEngine.Random.Range(0, Players.Length);
        randomIndex = 0;
        StarterPlayer = Players[randomIndex];
        CurrentPlayerTurn = StarterPlayer;
    }

    public void ChangeCurrentRound()
    {
        TurnCount++;
        Player aux = CurrentPlayerTurn;
        CurrentPlayerTurn = NextPlayerTurn;
        NextPlayerTurn = aux;
        OnChangeTurn?.Invoke();
    }

    public bool IsMyTurn(Player player)
    {
        return CurrentPlayerTurn == player;
    }

}

public class ActionsManager
{

    public void IncrementPlayerActions(Player player, int acAmount)
    {
        acAmount = 1;
        //  LE DOY UN PUNTO AL PLAYER
        player.ResetActionPoints(acAmount);
        //  RESETEO TODAS SUS HABILIDADES PARA QUE LAS PUEDA EJECUTAR
        foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.Abilities)
        {
            ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
        }
    }

    public void IncrementPlayerUnitsActions(Player player, int acAmount)
    {
        acAmount = 2;
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            player.kimbokoUnits[i].ResetActionPoints(acAmount);
            foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.kimbokoUnits[i].Abilities)
            {
                ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            }
        }
    }

    public void RestPlayerActions(Player player)
    {
        int acAmount = 0;
        //  LE DOY UN PUNTO AL PLAYER
        player.ResetActionPoints(acAmount);
        //  RESETEO TODAS SUS HABILIDADES PARA QUE LAS PUEDA EJECUTAR
        foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.Abilities)
        {
            ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.NONEXECUTABLE;
        }
    }

    public void RestPlayerUnitsActions(Player player)
    {
        int acAmount = 0;
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            player.kimbokoUnits[i].ResetActionPoints(acAmount);
            foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.kimbokoUnits[i].Abilities)
            {
                if (ab.Key == ABILITYTYPE.DIE || ab.Key == ABILITYTYPE.TAKEDAMAGE)
                {
                    continue;
                }
                ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.NONEXECUTABLE;
            }
        }
    }

    public bool DoesThePlayerHaveActionToExecute(Player player)
    {
        return player.GetCurrentActionPoints() <= 0;
    }

    public bool DoesThePlayerUnitsHaveActionsToExecute(Player player)
    {
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            if (player.kimbokoUnits[i].GetCurrentActionPoints() > 0)
            {
                return true;
            }
        }
        return false;
    }


}