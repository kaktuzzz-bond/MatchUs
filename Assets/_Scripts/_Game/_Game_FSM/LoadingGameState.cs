using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        if (context.IsExitGame)
        {
            LoadMainMenuScreenAsync().Forget();
        }
        else
        {
            LoadGameScreenAsync().Forget();
        }
    }


    public void Exit(GameFiniteStateMachine context) { }


    private static async UniTaskVoid LoadGameScreenAsync()
    {
        Debug.Log("Loading --> GAME ");

        await SceneManager.LoadSceneAsync(2);

        GameManager.Instance.GameFiniteStateMachine.Active();
    }


    private static async UniTaskVoid LoadMainMenuScreenAsync()
    {
        Debug.Log("Saving --> MAIN MENU ");

        await SceneManager.LoadSceneAsync(0);

        GameManager.Instance.GameFiniteStateMachine.Initial();
    }
}