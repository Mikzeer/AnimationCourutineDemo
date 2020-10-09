using UnityEngine;
using System;

public class GameTimer
{
    public float actualTime;
    string actualtime;
    public static event Action<string> OnTimeChange;
    bool pause = false;

    public bool running { get; private set; }

    public void Start(int actualTime)
    {
        // ACA COMENZAMOS A RESTAR EL TIEMPO
        GameCreator.Instance.SetTimer();
        this.actualTime = actualTime;
        running = true;
    }

    public void Stop()
    {
        actualTime = 0;
        running = false;
    }

    public void Pause()
    {
        //running = false;
        pause = true;
    }

    public void Resume()
    {
        //running = true;
        pause = false;
    }

    public void RestTime()
    {
        if (running == false) return;

        if (pause == true) return;

        actualTime -= Time.deltaTime;

        if (actualTime > 0)
        {
            var minutes = actualTime / 60; //Divide the guiTime by sixty to get the minutes.
            var seconds = actualTime % 60;//Use the euclidean division for the seconds.
            actualtime = string.Format("{0:00} : {1:00}", minutes, seconds);            
            OnTimeChange?.Invoke(actualtime);
        }

        if (actualTime <= 0)
        {
            Stop();
        }
    }

}
