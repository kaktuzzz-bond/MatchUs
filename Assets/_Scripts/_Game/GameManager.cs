using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(GameFiniteStateMachine))]
public class GameManager : Singleton<GameManager>
{
    public event Action OnGameStarted;

    public event Action OnGamePaused;

    public event Action OnGameResumed;

    public event Action OnGameOver;

    private DifficultyLevel _difficultyLevel;

    private GameFiniteStateMachine _gameFiniteStateMachine;

    public int ChipsOnStartNumber => GameConfig.GetChipsOnStart(_difficultyLevel);

    public float ChanceForRandom => GameConfig.GetChanceForRandom(_difficultyLevel);

    [ShowInInspector]
    public bool AllowInput { get; private set; }

    private int _score;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            GameGUI.Instance.UpdateScore(_score);
        }
    }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _gameFiniteStateMachine = GameFiniteStateMachine.Instance;
    }


    public void AddScore(int score)
    {
        Score += score;
    }


    public void StartLoading(DifficultyLevel difficultyLevel)
    {
        _difficultyLevel = difficultyLevel;

        _gameFiniteStateMachine.Loading();
    }


    public void StartGame()
    {
        Debug.LogWarning("GAME STARTED!");

        Score = 0;
        
        AllowInput = true;

        OnGameStarted?.Invoke();
    }


    public void PauseGame()
    {
        Debug.LogWarning("GAME PAUSED");

        AllowInput = false;
        
        _gameFiniteStateMachine.Pause();
        
        OnGamePaused?.Invoke();
    }


    public void ResumeGame()
    {
        Debug.LogWarning("GAME RESUMED!");

        AllowInput = true;
        
        OnGameResumed?.Invoke();
    }


    public void EndGame()
    {
        Debug.LogWarning("GAME OVER!");

        AllowInput = false;
        
        OnGameOver?.Invoke();
    }


    public void ExitGame()
    {
        AllowInput = false;
        _gameFiniteStateMachine.Loading();
    }
}