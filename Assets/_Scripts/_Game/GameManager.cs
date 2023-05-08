#define ENABLE_LOGS
using System;
using UnityEngine;

[RequireComponent(typeof(GameFiniteStateMachine))]
public class GameManager : Singleton<GameManager>
{
    public event Action OnGameStarted;
    
    public event Action OnGameEnded;
    
    private DifficultyLevel _difficultyLevel;

    private GameFiniteStateMachine _gameFiniteStateMachine;

    public int ChipsOnStartNumber =>
            _difficultyLevel switch
            {
                    DifficultyLevel.Test => 9,
                    DifficultyLevel.Easy => 27,
                    DifficultyLevel.Normal => 27,
                    DifficultyLevel.Hard => 45,
                    _ => throw new ArgumentOutOfRangeException()
            };

    public float ChanceForRandom =>
            _difficultyLevel switch
            {
                    DifficultyLevel.Test => 1.0f,
                    DifficultyLevel.Easy => 0.8f,
                    DifficultyLevel.Normal => 0.6f,
                    DifficultyLevel.Hard => 0.3f,
                    _ => throw new ArgumentOutOfRangeException()
            };


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        _gameFiniteStateMachine = GameFiniteStateMachine.Instance;
        
    }


    public void StartLoading(DifficultyLevel difficultyLevel)
    {
        _difficultyLevel = difficultyLevel;

        _gameFiniteStateMachine.Loading();
    }


    public void StartGame()
    {
        Logger.DebugWarning("GAME STARTED!");
        
        OnGameStarted?.Invoke();
    }
    
    public void EndGame()
    {
       Logger.DebugWarning("GAME OVER!");
       
       OnGameEnded?.Invoke();
    }
    
}