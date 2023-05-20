using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        PrepareToStart().Forget();
    }


    public void Exit(GameFiniteStateMachine context)
    {
        GameManager.Instance.DisableTimer();

        // int killed = DOTween.KillAll();
        //
        // Debug.LogWarning($"Killed: {killed}");

        LoadAsync().Forget();
    }


    private async UniTaskVoid PrepareToStart()
    {
        await CameraController.Instance.SetInitialPositionAsync();

        await CameraController.Instance.SetBoundsAsync();

        await GameGUI.Instance.SetupGUIAndFadeOut();
    }


    private async UniTaskVoid LoadAsync()
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity(50, 100);

        await SceneManager.LoadSceneAsync(1);
    }
}