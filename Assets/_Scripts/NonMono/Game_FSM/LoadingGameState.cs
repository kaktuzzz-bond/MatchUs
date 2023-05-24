using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonMono.Game_FSM
{
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


        private async UniTaskVoid LoadGameScreenAsync()
        {
            Debug.Log("Loading --> GAME");

            GameManager.Instance.gameData
                    .CalculateCameraOrthographicSize();

            await SceneManager
                    .LoadSceneAsync(2);

            GameManager.Instance.GameFiniteStateMachine
                    .Active();
        }


        private static async UniTaskVoid LoadMainMenuScreenAsync()
        {
            Debug.Log("Saving --> MAIN MENU");

            await SceneManager.LoadSceneAsync(0);

            GameManager.Instance.GameFiniteStateMachine.Initial();
        }
    }
}