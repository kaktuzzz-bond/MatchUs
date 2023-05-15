using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private DifficultyLevel _difficultyLevel;

    [ShowInInspector]
    public IGameState CurrentGameState => GameFiniteStateMachine.CurrentGameState;

    public GameFiniteStateMachine GameFiniteStateMachine { get; } = new();

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
    }


    public void AddScore(int score)
    {
        Score += score;
    }


    public void SetDifficultyAndLoad(DifficultyLevel difficultyLevel)
    {
        _difficultyLevel = difficultyLevel;

        GameFiniteStateMachine.Loading();
    }


    public void StartGame()
    {
        Debug.LogWarning("GAME STARTED!");

        ResetGameStats();

        EnableTimer();

        InputManager.Instance.SetPlayerInput(true);
    }


    public void PauseGame()
    {
        Debug.LogWarning("GAME PAUSED");

        GameFiniteStateMachine.Pause();
    }


    public void EndGame()
    {
        Debug.LogWarning("GAME OVER!");

        InputManager.Instance.SetPlayerInput(false);

        DisableTimer();
    }


    public void ExitGame()
    {
        GameFiniteStateMachine.Loading();
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