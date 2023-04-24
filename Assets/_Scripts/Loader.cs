using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new(0f);

    private GameStateManager _gameStateManager;


    private void Awake()
    {
        _gameStateManager = GameStateManager.Instance;
    }


    public void Start()
    {
        StartCoroutine(LoadingRoutine());
    }


    private IEnumerator LoadingRoutine()
    {
        Debug.Log(">>> LOADING...");

        yield return _wait;

        Debug.Log(" ...COMPLETED <<<");

        _gameStateManager.Active();
    }


    // private IEnumerator SavingRoutine()
    // {
    //     Debug.Log(">>> SAVING...");
    //
    //     yield return _wait;
    //
    //     Debug.Log(" ...COMPLETED <<<");
    //
    //     _gameStateManager.Prepare();
    // }
}