using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFiniteStateMachine : Singleton<GameFiniteStateMachine>
{
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }

    public bool IsExitGame { get; private set; }

    private readonly IGameState _initial = new InitialGameState();

    private readonly IGameState _loading = new LoadingGameState();

    private readonly IGameState _active = new ActiveGameState();

    private readonly IGameState _pause = new PauseGameState();


    private void Start()
    {
        Initial();
    }


    public void Initial()
    {
        IsExitGame = false;

        SetState(_initial);
    }


    public void Loading()
    {
        SetState(_loading);
    }


    public void Active()
    {
        SetState(_active);

        IsExitGame = true;
    }


    public void Pause()
    {
        SetState(_pause);
    }


    private void SetState(IGameState newGameState)
    {
        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
    }
}