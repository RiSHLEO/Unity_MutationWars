using UnityEngine;
using UnityEngine.Playables;

public class StateMachine
{
    public EntityState currentState { get; private set; }

    public void Initialize(EntityState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void ReEnterCurrentState()
    {
        currentState.Enter();
        currentState.Exit();
    }
}