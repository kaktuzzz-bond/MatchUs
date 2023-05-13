using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    private const int LoadingSceneIndex = 1;


    public void Enter(GameFiniteStateMachine context)
    {
        if (context.IsExitGame)
        {
            LoadMainMenuScreen().Forget();
        }
        else
        {
           LoadGameScreen().Forget();
        }
    }


    private static async UniTaskVoid LoadMainMenuScreen()
    {
        Debug.Log("Loading --> MAIN MENU ");

        DOTween.KillAll();

        await UniTask.Delay(1000);

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);

        GameFiniteStateMachine.Instance.Initial();
    }


    private static async UniTaskVoid LoadGameScreen()
    {
        Debug.Log("Loading --> GAME ");

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);

        await UniTask.Delay(1000);

        GameFiniteStateMachine.Instance.Active();
    }
}