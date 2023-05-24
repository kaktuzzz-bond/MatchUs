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
        public static int Counter => InGameChips.Count;

        private static readonly List<Chip> InGameChips = new();

        public static List<Chip> ActiveChips => InGameChips
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

            InGameChips.Add(chip);
        }


        public static void UnregisterFromGame(Chip chip)
        {
            InGameChips.Remove(chip);

            CheckCounter();
        }


        public static async UniTask UnregisterAndDestroy(Chip chip)
        {
            await chip.RemoveFromBoardAsync();

            AllChips.Remove(chip);

            InGameChips.Remove(chip);

            chip.Destroy();

            CheckCounter();
        }


        public static List<Chip> GetChipsBelowLine(int boardLine)
        {
            return InGameChips.Where(chip => chip.BoardPosition.y > boardLine).ToList();
        }


        public static List<Chip> GetChipsOnLineAndBelow(int boardLine)
        {
            return InGameChips.Where(chip => chip.BoardPosition.y >= boardLine).ToList();
        }


        public static async UniTaskVoid Reset()
        {
            List<UniTask> tasks = new();

            foreach (Chip chip in AllChips)
            {
                tasks.Add(DestroyOnReset(chip));
            }

            await UniTask.WhenAll(tasks);

            InGameChips.Clear();

            AllChips.Clear();
        }


        private static async UniTask DestroyOnReset(Chip chip)
        {
            await chip.RemoveFromBoardAsync();

            chip.Destroy();
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
            int emptyCells = GameManager.Instance.gameData.Board.Capacity - InGameChips.Count;

            GameGUI.Instance.AddButton.SetInteractivity(emptyCells >= Counter);
        }
    }
}