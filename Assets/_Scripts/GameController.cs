using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private DifficultyLevel _difficultyLevel;

    private GameStateManager _gameStateManager;

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
                    DifficultyLevel.Normal => 0.0f,
                    DifficultyLevel.Hard => 1.0f,
                    _ => throw new ArgumentOutOfRangeException()
            };


    private void Awake()
    {
        _gameStateManager = GameStateManager.Instance;
    }


    public void StartGame(DifficultyLevel difficultyLevel)
    {
        _difficultyLevel = difficultyLevel;

        _gameStateManager.Loading();
    }
}