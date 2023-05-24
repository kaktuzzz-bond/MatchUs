using Cysharp.Threading.Tasks;
using Game;
using NonMono.Commands;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonMono.Game_FSM
{
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
            //DOTween.SetTweensCapacity(5000, 100);

            await Board.Board.Instance.Init();

            await CameraController.Instance
                    .SetOrthographicSizeAsync();

            await CameraController.Instance.SetBoundsAsync();

            await CameraController.Instance.SetInitialPositionAsync();

            await CameraController.Instance.SetBoundsAsync();

            await CameraController.Instance.SetInitialPositionAsync();

            await GameGUI.Instance.SetupGUIAndFadeOut();

            await UniTask.Delay(200);

            await CommandLogger
                    .AddCommand(new AddChipsCommand(GameManager.Instance.gameData.StartArrayInfos));

            await UniTask.Delay(100);

            GameManager.Instance.StartGame();
        }
    }
}