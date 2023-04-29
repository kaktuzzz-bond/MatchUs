using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneGUI : Singleton<MainSceneGUI>
{
    [SerializeField] [TabGroup("Main Menu")]
    private Button easyMode;

    [SerializeField] [TabGroup("Main Menu")]
    private Button normalMode;

    [SerializeField] [TabGroup("Main Menu")]
    private Button hardMode;

    [SerializeField] [TabGroup("Main Menu")]
    private Button testMode;

    private GameController _gameController;


    private void Awake()
    {
        _gameController = GameController.Instance;

        easyMode.onClick.AddListener(() => _gameController.StartGame(DifficultyLevel.Easy));

        normalMode.onClick.AddListener(() => _gameController.StartGame(DifficultyLevel.Normal));

        hardMode.onClick.AddListener(() => _gameController.StartGame(DifficultyLevel.Hard));

        testMode.onClick.AddListener(() => _gameController.StartGame(DifficultyLevel.Test));
    }
}