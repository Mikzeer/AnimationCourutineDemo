using PositionerDemo;
using System;

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
        if (randomIndex == Players.Length)
        {
            NextPlayerTurn = Players[0];
        }
        else
        {
            NextPlayerTurn = Players[1];
        }
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
