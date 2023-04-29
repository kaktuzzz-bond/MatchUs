using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }


    private void Start()
    {
        CurrentGameState = new InitialGameState();
        CurrentGameState.Enter(this);
    }


    public void Initial()
    {
        SetState(new InitialGameState());
    }


    public void Loading()
    {
        SetState(new LoadingGameState());
    }


    public void Active()
    {
        SetState(new ActiveGameState());
    }


    public void Pause()
    {
        SetState(new PauseGameState());
    }


    public void Exit()
    {
        SetState(new ExitGameState());
    }


    private void SetState(IGameState newGameState)
    {
        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
    }
}