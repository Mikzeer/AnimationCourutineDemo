using UnityEngine;
using System;

public class GameTimer
{
    public float actualTime;
    string actualtime;
    public Action<string> OnTimePass;
    bool pause = false;
    public bool running { get; private set; }

    public GameTimer(int actualTime)
    {
        this.actualTime = actualTime;
    }

    public void Start()
    {
        running = true;
    }

    public void Stop()
    {
        actualTime = 0;
        running = false;
    }

    public void Pause()
    {
        pause = true;
    }

    public void Resume()
    {
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

            if (minutes < 1)
            {
                var seconds = actualTime % 60;//Use the euclidean division for the seconds.
                actualtime = string.Format("{0:00}", seconds);
            }
            else
            {
                var seconds = actualTime % 60;//Use the euclidean division for the seconds.
                actualtime = string.Format("{0:00} : {1:00}", minutes, seconds);
            }        
            OnTimePass?.Invoke(actualtime);
        }

        if (actualTime <= 0)
        {
            Stop();
        }
    }

}