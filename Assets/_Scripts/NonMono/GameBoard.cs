using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NonMono
{
    public class GameBoard
    {
        public int Capacity { get; private set; }

        private const float DotRandomizationValue = 0.15f;

        private readonly int _width;

        private readonly int _height;

        private readonly Vector3[,] _tilePositions;

        private readonly List<Vector3> _dots = new();


        public GameBoard(int width, int height)
        {
            _width = width;

            _height = height;

            Capacity = width * height;

            _tilePositions = GetTilePositions(width, height);
        }


        public Vector3 this[int x, int y] => _tilePositions[x, y];


        private Vector3[,] GetTilePositions(int width, int height)
        {
            var positions = new Vector3[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    positions[x, y] = new Vector3(x, -y, 0f);

                    DrawDot(x, y);
                }
            }

            return positions;
        }


        private void DrawDot(int x, int y)
        {
            if (x == _width - 1 ||
                y == _height - 1 ||
                Random.value > DotRandomizationValue)
            {
                return;
            }

            float dotX = x + 0.5f;

            float dotY = -(y + 0.5f);

            _dots.Add(new Vector3(dotX, dotY, 0));
        }


        public async UniTask DrawBoardAsync()
        {
            GameManager gameManager = GameManager.Instance;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Transform tile = Object.Instantiate(
                            gameManager.gameData.tilePrefab,
                            this[x, y],
                            Quaternion.identity,
                            gameManager.gameData.tileParent);

                    tile.name = $"Tile({x}, {y})";
                }
            }

            await DrawDots();

            Debug.Log("Board has been drawn");
        }


        private async UniTask DrawDots()
        {
            GameManager gameManager = GameManager.Instance;

            foreach (Vector3 pos in _dots)
            {
                Transform dot = Object.Instantiate(
                        gameManager.gameData.dotPrefab,
                        pos,
                        Quaternion.identity,
                        gameManager.gameData.tileParent);

                dot.name = "Dot";

                SpriteRenderer sr = dot.GetComponentInChildren<SpriteRenderer>();

                sr.color = gameManager.gameData.GetRandomColor();
            }

            await UniTask.Yield();
        }
    }
}