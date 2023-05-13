using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermediateGameState : IGameState
{
    private const int LoadingSceneIndex = 1;


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


    public void Exit(GameFiniteStateMachine context)
    {
        //throw new System.NotImplementedException();
    }


    private static async UniTaskVoid LoadGameScreenAsync()
    {
        Debug.Log("Loading --> GAME ");

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);

        await UniTask.Delay(1000);

        GameFiniteStateMachine.Instance.Active();
    }


    private static async UniTaskVoid LoadMainMenuScreenAsync()
    {
        Debug.Log("Saving --> MAIN MENU ");

        InputManager.Instance.DisablePlayerInput();
        
        DOTween.KillAll();

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);
        
        await UniTask.Delay(1000);

        GameFiniteStateMachine.Instance.Initial();
    }
}