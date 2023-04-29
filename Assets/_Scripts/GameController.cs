using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public event Action OnGameStarted;

    private bool _isGameStarted;

    private GameSceneGUI _sceneGUI;

    public DifficultyLevel DifficultyLevel { get; private set; }

    public int ChipsOnStartNumber =>
            DifficultyLevel switch
            {
                    DifficultyLevel.Test => 9,
                    DifficultyLevel.Easy => 27,
                    DifficultyLevel.Normal => 27,
                    DifficultyLevel.Hard => 45,
                    _ => throw new ArgumentOutOfRangeException()
            };

    public float ChanceForRandom =>
            DifficultyLevel switch
            {
                    DifficultyLevel.Test => 1.0f,
                    DifficultyLevel.Easy => 0.9f,
                    DifficultyLevel.Normal => 0.6f,
                    DifficultyLevel.Hard => 1f,
                    _ => throw new ArgumentOutOfRangeException()
            };


    private void Awake()
    {
        _sceneGUI = GameSceneGUI.Instance;
    }


    private void StartGame()
    {
        DifficultyLevel = GameStateManager.Instance.CurrentDifficultyLevel;

        _isGameStarted = true;

        OnGameStarted?.Invoke();
    }


#region Enable / Disable

    private void OnEnable()
    {
        _sceneGUI.OnFadedIn += StartGame;
    }


    private void OnDisable()
    {
        _sceneGUI.OnFadedIn -= StartGame;
    }

#endregion
}