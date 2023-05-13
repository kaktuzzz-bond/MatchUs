using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    private const int LoadingSceneIndex = 2;


    public void Enter(GameFiniteStateMachine context)
    {
        LoadAsync().Forget();
    }


    public void Exit(GameFiniteStateMachine context)
    {
        //throw new System.NotImplementedException();
    }


    private static async UniTaskVoid LoadAsync()
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity(5000, 1000);

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);
    }
}