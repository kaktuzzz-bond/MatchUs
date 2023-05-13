using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameFiniteStateMachine : Singleton<GameFiniteStateMachine>
{
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }

    public bool IsExitGame { get; private set; }

    private readonly InitialGameState _initial = new ();

    private readonly LoadingGameState _loading = new ();

    public ActiveGameState ActiveGame { get; } = new ();

    public PauseGameState PauseGame { get; } = new ();


    private void Start()
    {
        CurrentGameState = _initial;
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
        SetState(ActiveGame);

        IsExitGame = true;
    }


    public void Pause()
    {
        SetState(PauseGame);
    }


    private void SetState(IGameState newGameState)
    {
        CurrentGameState.Exit(this);

        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
    }
}