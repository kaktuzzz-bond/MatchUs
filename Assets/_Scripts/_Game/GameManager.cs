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

    private int _score;

    private float _timerCounter;

    private bool _isTimerOn;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            GameGUI.Instance.UpdateScore(_score);
        }
    }


    private void Update()
    {
        CountTime();
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


    public void SetDifficultyAndLoad(DifficultyLevel difficultyLevel)
    {
        _difficultyLevel = difficultyLevel;

        _gameFiniteStateMachine.Loading();
    }


    public void StartGame()
    {
        Debug.LogWarning("GAME STARTED!");

        ResetGameStats();

        EnableTimer();

        InputManager.Instance.EnablePlayerInput();
    }


    public void PauseGame()
    {
        Debug.LogWarning("GAME PAUSED");

        _gameFiniteStateMachine.Pause();
    }


    public void EndGame()
    {
        Debug.LogWarning("GAME OVER!");

        InputManager.Instance.DisablePlayerInput();

        DisableTimer();
    }


    public void ExitGame()
    {
        _gameFiniteStateMachine.Loading();
    }


    public void EnableTimer() => _isTimerOn = true;


    public void DisableTimer() => _isTimerOn = false;


    private void ResetGameStats()
    {
        Score = 0;

        _timerCounter = 0;
    }


    private void CountTime()
    {
        if (!_isTimerOn) return;

        _timerCounter += Time.deltaTime;

        GameGUI.Instance.UpdateTime(_timerCounter);
    }
}