using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{ 
    public event Action OnGameOver;


    [SerializeField]
    private GameData gameData;
    
    private DifficultyLevel _difficultyLevel;

    [ShowInInspector]
    public IGameState CurrentGameState => GameFiniteStateMachine.CurrentGameState;

    public GameFiniteStateMachine GameFiniteStateMachine { get; } = new();

    // public int ChipsOnStartNumber => GameConfig.GetChipsOnStart(_difficultyLevel);
    //
    // public float ChanceForRandom => GameConfig.GetChanceForRandom(_difficultyLevel);

    public bool IsGameActive { get; private set; }

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


    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }

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
        Debug.Log("GAME STARTED!");

        ResetGameStats();

        EnableTimer();

        InputManager.Instance.SetPlayerInput(true);
    }


    public void PauseGame()
    {
        Debug.Log("GAME PAUSED");

        DisableTimer();
    }

    public void ResumeGame()
    {
        Debug.Log("GAME RESUMED");

        EnableTimer();
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER!");

        OnGameOver?.Invoke();
        
        InputManager.Instance.SetPlayerInput(false);

        DisableTimer();
    }


    public void ExitGame()
    {
        GameFiniteStateMachine.Loading();
    }


    private void EnableTimer() => CountTimeAsync().Forget();


    public void DisableTimer() => _isTimerOn = false;


    private void ResetGameStats()
    {
        Score = 0;

        _timerCounter = 0;
    }


    private async UniTaskVoid CountTimeAsync()
    {
        if (CurrentGameState.GetType() != typeof(ActiveGameState)) return;

        Debug.Log("Timer starts");

        _isTimerOn = true;

        while (_isTimerOn)
        {
            _timerCounter++;

            try
            {
                GameGUI.Instance.UpdateTime(_timerCounter);
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning(e);

                DisableTimer();
            }

            await UniTask.Delay(1000);
        }

        Debug.Log("Timer stopped");
    }
}