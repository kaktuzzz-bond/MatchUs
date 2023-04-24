using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }

    [ShowInInspector]
    public DifficultyLevel CurrentDifficultyLevel { get; private set; }

    private MainSceneGUI _mainSceneGUI;


    private void Awake()
    {
        _mainSceneGUI = MainSceneGUI.Instance;
    }


    private void Start()
    {
        CurrentGameState = new InitialGameState();
        CurrentGameState.Enter(this);
    }


    public void Initial() => SetState(new InitialGameState());


    public void Loading(DifficultyLevel difficultyLevel)
    {
        CurrentDifficultyLevel = difficultyLevel;
        SetState(new LoadingGameState());
    }


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
        _mainSceneGUI.OnDifficultyLevelSelected += Loading;
    }


    private void OnDisable()
    {
        _mainSceneGUI.OnDifficultyLevelSelected -= Loading;
    }

#endregion
}