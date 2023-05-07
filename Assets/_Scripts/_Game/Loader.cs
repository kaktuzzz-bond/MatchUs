#define ENABLE_LOGS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new(0f);


    public void Start()
    {
        StartCoroutine(LoadingRoutine());
    }


    private IEnumerator LoadingRoutine()
    {
        Logger.Debug(">>> LOADING...");

        yield return _wait;

        Logger.Debug(" ...COMPLETED <<<");

        GameFiniteStateMachine.Instance.CurrentGameState.Exit(GameFiniteStateMachine.Instance);
    }


    // private IEnumerator SavingRoutine()
    // {
    //     Debug.Log(">>> SAVING...");
    //
    //     yield return _wait;
    //
    //     Debug.Log(" ...COMPLETED <<<");
    //
    //     _gameFiniteStateMachine.Prepare();
    // }
}