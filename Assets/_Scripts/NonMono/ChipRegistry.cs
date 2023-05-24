using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace NonMono
{
    public static class ChipRegistry
    {
        [ShowInInspector, ReadOnly]
        public static int Counter => _inGameChips.Count;

        private static List<Chip> _inGameChips = new();

        public static List<Chip> ActiveChips => _inGameChips
                .Where(c => c.CurrentChipState == ChipState.LightOn)
                .OrderBy(c => c.BoardPosition.y)
                .ThenBy(c => c.BoardPosition.x)
                .ToList();

        private static readonly List<Chip> AllChips = new();


        public static void RegisterInGame(Chip chip)
        {
            if (!AllChips.Contains(chip))
            {
                AllChips.Add(chip);
            }

            _inGameChips.Add(chip);

            Debug.LogWarning(_inGameChips.Count);
        }


        public static void UnregisterFromGame(Chip chip)
        {
            _inGameChips.Remove(chip);

            CheckCounter();

            Debug.LogError(_inGameChips.Count);
        }


        public static async UniTask UnregisterAndDestroy(Chip chip)
        {
            AllChips.Remove(chip);

            _inGameChips.Remove(chip);

            await chip.RemoveFromBoardAsync();

            chip.Destroy();

            CheckCounter();
        }


        public static List<Chip> GetChipsBelowLine(int boardLine)
        {
            return _inGameChips.Where(chip => chip.BoardPosition.y > boardLine).ToList();
        }


        public static List<Chip> GetChipsOnLineAndBelow(int boardLine)
        {
            return _inGameChips.Where(chip => chip.BoardPosition.y >= boardLine).ToList();
        }


        public static async UniTaskVoid Reset()
        {
            List<UniTask> tasks = new();

            foreach (Chip chip in AllChips)
            {
                tasks.Add(UnregisterAndDestroy(chip));
            }

            await UniTask.WhenAll(tasks);

            _inGameChips.Clear();

            AllChips.Clear();
        }


        private static void CheckCounter()
        {
            if (Counter == 0)
            {
                GameManager.Instance.EndGame();
            }
        }


        public static void CheckBoardCapacity()
        {
            int emptyCells = GameManager.Instance.gameData.Board.Capacity - _inGameChips.Count;

            GameGUI.Instance.AddButton.SetInteractivity(emptyCells >= Counter);
        }
    }
}