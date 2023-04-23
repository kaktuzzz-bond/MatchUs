using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new(2f);

    private GameStateManager gameStateManager;


    private void Awake()
    {
        gameStateManager = GameStateManager.Instance;
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

        gameStateManager.Active();
    }


    // private IEnumerator SavingRoutine()
    // {
    //     Debug.Log(">>> SAVING...");
    //
    //     yield return _wait;
    //
    //     Debug.Log(" ...COMPLETED <<<");
    //
    //     gameStateManager.Prepare();
    // }
}