using System;

public class TimeConditionState : State
{
    public GameCreator gameCreator { get; set; }
    public bool endState { get; set; }
    public int duration;
    string actualStateName;
    //public static event Action<string> OnTimeChange;
    public static event Action<string> OnStateChange;
    public GameTimer gameTimer;

    public TimeConditionState(int duration, GameCreator gameCreator, string actualStateName)
    {
        this.duration = duration;
        this.gameCreator = gameCreator;
        this.actualStateName = actualStateName;
        gameTimer = new GameTimer();
    }

    public virtual void Enter()
    {
        OnStateChange?.Invoke(actualStateName);
        gameTimer.Start(duration);
    }

    public virtual void Exit()
    {
        gameTimer.Stop();
    }

    public virtual State Update()
    {
        gameTimer.RestTime();

        return null;
    }

    public virtual void GetBack()
    {
        OnStateChange?.Invoke(actualStateName);
    }

    public virtual bool CheckCondition()
    {
        return false;
    }

}