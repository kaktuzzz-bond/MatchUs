using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        // int killed = DOTween.KillAll();
        //
        // Debug.LogWarning($"Killed: {killed}");
        
        LoadAsync().Forget();
    }


    private async UniTaskVoid LoadAsync()
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity((int)(Board.Instance.BoardCapacity * 0.5f), 100);

        await SceneManager.LoadSceneAsync(1);
    }
}