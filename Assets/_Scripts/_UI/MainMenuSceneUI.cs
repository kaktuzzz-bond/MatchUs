using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneUI : Singleton<MainMenuSceneUI>
{
    [SerializeField, TabGroup("Main")]
    private Button easyMode;

    [SerializeField, TabGroup("Main")]
    private Button normalMode;

    [SerializeField, TabGroup("Main")]
    private Button hardMode;

    [SerializeField, TabGroup("Main")]
    private Button testMode;

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;

        easyMode.onClick.AddListener(() => _gameManager.StartGame(DifficultyLevel.Easy));

        normalMode.onClick.AddListener(() => _gameManager.StartGame(DifficultyLevel.Normal));

        hardMode.onClick.AddListener(() => _gameManager.StartGame(DifficultyLevel.Hard));

        testMode.onClick.AddListener(() => _gameManager.StartGame(DifficultyLevel.Test));
    }
}