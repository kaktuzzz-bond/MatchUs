using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    private void Start()
    {
        CurrentGameState = new InitialGameState();
        CurrentGameState.Enter(this);
    }


    public void Initial() => SetState(new InitialGameState());


    public void Loading() => SetState(new LoadingGameState());


    public void Active() => SetState(new ActiveGameState());


    public void Pause() => SetState(new PauseGameState());


    public void Exit() => SetState(new ExitGameState());


    public void SetState(IGameState newGameState)
    {
        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
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