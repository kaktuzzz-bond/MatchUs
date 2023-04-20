using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : Singleton<GameManager>
{
    public DifficultyLevel Difficulty { get; private set; }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void StartGame(DifficultyLevel difficultyLevel)
    {
        Difficulty = difficultyLevel;

        GameStateMachine.Instance.LoadData();
    }
}