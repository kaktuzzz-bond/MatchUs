using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NonMono;
using UnityEngine;

namespace Board
{
    public class Board : Singleton<Board>
    {
        private HashSet<UniTask> _chipTasksAll = new();

        private GameBoard _gameBoard;

        private GameManager _gameManager;

        public int Capacity => _gameBoard.Capacity;

        private PointerPool _pointerPool;


        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }


        public async UniTask Init()
        {
            _gameManager.gameData.tileParent = CreateParent("Tile");
            _gameManager.gameData.chipParent = CreateParent("Chips");
            _gameManager.gameData.pointerParent = CreateParent("Pointers");

            _gameBoard = new(
                    _gameManager.gameData.width,
                    _gameManager.gameData.height);

            LineChecker.SetBoard(_gameBoard);

            _pointerPool = new PointerPool();
            _pointerPool.Init();

            await _gameBoard.DrawBoardAsync();
        }


        private Transform CreateParent(string parentName)
        {
            GameObject go = new();

            Transform parent = go.transform;

            parent.name = parentName;

            parent.SetParent(transform);

            return parent;
        }


        public Vector3 this[int x, int y] => _gameBoard[x, y];


        public void ShowHints(Vector3 firstPosition, Vector3 secondPosition)
        {
            _pointerPool.ShowHints(firstPosition, secondPosition);
        }


        public void HideHints()
        {
            _pointerPool.HideHints();
        }


        public void ShowSelector(Vector3 position)
        {
            _pointerPool.ShowSelector(position);
        }


        public void HideSelector()
        {
            _pointerPool.HideSelector();
        }


        public static Vector2Int GetBoardPosition(int count)
        {
            int width = GameManager.Instance.gameData.width;

            return new Vector2Int(count % width, count / width);
        }
    }
}