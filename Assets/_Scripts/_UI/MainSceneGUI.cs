using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneGUI : Singleton<MainSceneGUI>
{
    public event Action<DifficultyLevel> OnDifficultyLevelSelected;

    [SerializeField, TabGroup("Main Menu")]
    private Button easyMode;

    [SerializeField, TabGroup("Main Menu")]
    private Button normalMode;

    [SerializeField, TabGroup("Main Menu")]
    private Button hardMode;

    [SerializeField, TabGroup("Main Menu")]
    private Button testMode;

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;

        easyMode.onClick.AddListener(() => OnDifficultyLevelSelected?.Invoke(DifficultyLevel.Easy));

        normalMode.onClick.AddListener(() => OnDifficultyLevelSelected?.Invoke(DifficultyLevel.Normal));

        hardMode.onClick.AddListener(() => OnDifficultyLevelSelected?.Invoke(DifficultyLevel.Hard));

        testMode.onClick.AddListener(() => OnDifficultyLevelSelected?.Invoke(DifficultyLevel.Test));
    }
}