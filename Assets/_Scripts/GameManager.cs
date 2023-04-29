using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(GameStateManager))]
public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}