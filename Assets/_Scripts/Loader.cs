using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new(2f);

    private GameStateMachine _gameStateMachine;


    private void Awake()
    {
        _gameStateMachine = GameStateMachine.Instance;
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

        _gameStateMachine.GoToActive();
    }


    // private IEnumerator SavingRoutine()
    // {
    //     Debug.Log(">>> SAVING...");
    //
    //     yield return _wait;
    //
    //     Debug.Log(" ...COMPLETED <<<");
    //
    //     _gameStateMachine.Prepare();
    // }
}