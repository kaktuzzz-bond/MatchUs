using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Board.Instance.StartingBoard();
    }


    public void Exit(GameFiniteStateMachine context)
    {
        GameManager.Instance.DisableTimer();

        DOTween.KillAll();

        LoadAsync().Forget();
    }


    private static async UniTaskVoid LoadAsync()
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity(2500, 500);

        await SceneManager.LoadSceneAsync(1);
    }
}