using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateMachine : Singleton<GameStateMachine>, IStateContext
{
    public IState CurrentState { get; private set; } = new InitialGameState();


    private void Start()
    {
        CurrentState.Enter(this);
    }


    public void GoToInitial() => SetState(new InitialGameState());


    public void GoToLoading() => SetState(new LoadingGameState());


    public void GoToActive() => SetState(new ActiveGameState());


    public void GoToPause() => SetState(new PauseGameState());


    public void GoToExit() => SetState(new ExitGamState());


    public void SetState(IState newState)
    {
        CurrentState.Exit(this);

        CurrentState = newState;

        CurrentState.Enter(this);
    }
}