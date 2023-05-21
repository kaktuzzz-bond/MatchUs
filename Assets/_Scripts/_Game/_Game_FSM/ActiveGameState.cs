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
        Debug.LogWarning("Saving system should be here");

        GameManager.Instance.DisableTimer();

        LoadAsync().Forget();
    }


    private async UniTaskVoid LoadAsync()
    {
        await SceneManager.LoadSceneAsync(1);
    }


    private async UniTaskVoid PrepareToStart()
    {
        DOTween.SetTweensCapacity(5000, 100);

        GameBoard gameBoard = new(
                GameManager.Instance.gameData.width,
                GameManager.Instance.gameData.height);

        Board.Instance.Init(gameBoard);

        await CameraController.Instance
                .SetOrthographicSizeAsync();

        await gameBoard
                .DrawBoardAsync();

        await CameraController.Instance.SetBoundsAsync();

        await CameraController.Instance.SetInitialPositionAsync();

        await CameraController.Instance.SetBoundsAsync();

        await CameraController.Instance.SetInitialPositionAsync();

        await GameGUI.Instance.SetupGUIAndFadeOut();

        await UniTask.Delay(200);
        
        await ChipController.Instance
                .DrawArrayAsync(GameManager.Instance.gameData.StartArrayInfos);

        await UniTask.Delay(100);

        GameManager.Instance.StartGame();
    }
}