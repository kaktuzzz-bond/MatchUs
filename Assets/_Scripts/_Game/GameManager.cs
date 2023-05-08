#define ENABLE_LOGS
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(GameFiniteStateMachine))]
public class GameManager : Singleton<GameManager>
{
    private DifficultyLevel _difficultyLevel;

    private GameFiniteStateMachine _gameFiniteStateMachine;

    public int ChipsOnStartNumber => GameConfig.GetChipsOnStart(_difficultyLevel);
          

    public float ChanceForRandom => GameConfig.GetChanceForRandom(_difficultyLevel);


    [ShowInInspector]
    public bool AllowInput { get; private set; }

    private int _score;
    
    private float _timerCounter;

    private bool _timerOn;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _gameFiniteStateMachine = GameFiniteStateMachine.Instance;
    }


    private void Update()
    {
        CountTime();
    }


    public void AddScore(int score)
    {
        _score += score;
        
        GameGUI.Instance.UpdateScore(_score);
    }

    private void CountTime()
    {
        if (!_timerOn) return;

        _timerCounter += Time.deltaTime;

        GameGUI.Instance.UpdateTime(_timerCounter);
    }


    public void StartLoading(DifficultyLevel difficultyLevel)
    {
        Logger.DebugWarning("LOADING>>>");
        
        _difficultyLevel = difficultyLevel;

        _gameFiniteStateMachine.Loading();
    }


    public void StartGame()
    {
        Logger.DebugWarning("GAME STARTED!");

        _timerCounter = 0;

        _score = 0;
        
        _timerOn = true;

        AllowInput = true;
    }


    public void PauseGame()
    {
        Logger.DebugWarning("GAME PAUSED");
        
        _timerOn = false;
        
        AllowInput = false;
    }
    
    
    public void ResumeGame()
    {
        Logger.DebugWarning("GAME RESUMED!");
        
        _timerOn = true;
        
        AllowInput = true;
    }
    
    public void EndGame()
    {
        Logger.DebugWarning("GAME OVER!");
        
        _timerOn = false;
        
        AllowInput = false;
    }
}