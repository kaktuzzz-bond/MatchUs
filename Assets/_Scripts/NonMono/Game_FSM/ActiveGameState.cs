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
        GameManager _gameManager;


        public void Enter(GameFiniteStateMachine context)
        {
            _gameManager = GameManager.Instance;

            OrganizeFolders(CameraController.Instance.transform);

            _gameManager.gameData.SetBoard(
                    new(
                            _gameManager.gameData.width,
                            _gameManager.gameData.height));

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
            await _gameManager.gameData.Board.DrawBoardAsync();

            await CameraController.Instance
                    .SetOrthographicSizeAsync();

            await CameraController.Instance.SetBoundsAsync();

            await CameraController.Instance.SetInitialPositionAsync();

            await GameGUI.Instance.SetupGUIAndFadeOut();

            await UniTask.Delay(200);

            await CommandLogger
                    .AddCommand(new AddChipsCommand(GameManager.Instance.gameData.StartArrayInfos));

            await UniTask.Delay(100);

            GameManager.Instance.StartGame();
        }


        private void OrganizeFolders(Transform parent)
        {
            GameManager gm = GameManager.Instance;
            gm.gameData.tileParent = CreateParent("Tile", parent);
            gm.gameData.chipParent = CreateParent("Chips", parent);
            gm.gameData.pointerParent = CreateParent("Pointers", parent);
        }


        private Transform CreateParent(string folderName, Transform parent)
        {
            GameObject go = new();

            Transform folder = go.transform;

            folder.name = folderName;

            folder.SetParent(parent);

            return folder;
        }
    }
}