using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStateManager))]
public class GameManager : Singleton<GameManager>
{
    public event Action OnDifficultySelected; 
    public DifficultyLevel Difficulty { get; private set; }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void StartGame(DifficultyLevel difficultyLevel)
    {
        Difficulty = difficultyLevel;

        OnDifficultySelected?.Invoke();
    }
}