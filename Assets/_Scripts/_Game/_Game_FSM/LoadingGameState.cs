using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    private readonly GameManager _gameManager;
    
    public LoadingGameState()
    {
        _gameManager = GameManager.Instance;
        
    }


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


    private async UniTaskVoid LoadGameScreenAsync()
    {
        Debug.Log("Loading --> GAME");
        
        _gameManager.gameData.CalculateCameraOrthographicSize();

        await SceneManager.LoadSceneAsync(2);

        Board.Instance.Init(
                new GameBoard(
                        _gameManager.gameData.width,
                        _gameManager.gameData.height));
        
        await CameraController.Instance.SetOrthographicSizeAsync();

        await Board.Instance.DrawBoardAsync();
        
        GameManager.Instance.GameFiniteStateMachine.Active();
    }


    private static async UniTaskVoid LoadMainMenuScreenAsync()
    {
        Debug.Log("Saving --> MAIN MENU");

        await SceneManager.LoadSceneAsync(0);

        GameManager.Instance.GameFiniteStateMachine.Initial();
    }


    
   
}