using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public State currentState;

    public void Initialize(State states)
    {
        currentState = states;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            State nextState = currentState.Update();
            ChangeState(nextState);
        }
        else
        {
            //Debug.Log("No hay State para Ejecutar");
        }

    }

    public void ChangeState(State nextState)
    {
        if (nextState != null)
        {
            currentState.Exit();
            nextState.Enter();
            currentState = nextState;
        }
    }

    public void GetBack(State previousState)
    {
        if (previousState != null)
        {
            currentState.Exit();
            previousState.GetBack();
            currentState = previousState;
        }
    }
}