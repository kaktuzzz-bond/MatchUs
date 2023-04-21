using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateMachine : Singleton<GameStateMachine>, IStateContext
{
    [ShowInInspector]
    public IState CurrentState { get; private set; } = new InitialGameState();

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    private void Start()
    {
        CurrentState.Enter(this);
    }


    public void Initial() => SetState(new InitialGameState());


    public void Loading() => SetState(new LoadingGameState());


    public void Active() => SetState(new ActiveGameState());


    public void Pause() => SetState(new PauseGameState());


    public void Exit() => SetState(new ExitGamState());


    public void SetState(IState newState)
    {
        CurrentState.Exit(this);

        CurrentState = newState;

        CurrentState.Enter(this);
    }


    #region Enable / Disable

    private void OnEnable()
    {
        _gameManager.OnDifficultySelected += Loading;
    }


    private void OnDisable()
    {
        _gameManager.OnDifficultySelected -= Loading;
    }

    #endregion
}