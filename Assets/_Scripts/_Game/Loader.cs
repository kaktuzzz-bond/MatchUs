using System.Collections;
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
        Debug.Log(">>> LOADING...");

        yield return _wait;

        Debug.Log(" ...COMPLETED <<<");

        GameFiniteStateMachine.Instance.Active();
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