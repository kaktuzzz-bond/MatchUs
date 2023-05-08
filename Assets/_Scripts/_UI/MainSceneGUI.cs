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

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;

        easyMode.onClick.AddListener(() => _gameManager.StartLoading(DifficultyLevel.Easy));

        normalMode.onClick.AddListener(() => _gameManager.StartLoading(DifficultyLevel.Normal));

        hardMode.onClick.AddListener(() => _gameManager.StartLoading(DifficultyLevel.Hard));

        testMode.onClick.AddListener(() => _gameManager.StartLoading(DifficultyLevel.Test));
    }
}