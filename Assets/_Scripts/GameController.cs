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