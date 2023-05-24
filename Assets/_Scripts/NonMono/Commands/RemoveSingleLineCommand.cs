using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;

namespace NonMono.Commands
{
    public class RemoveSingleLineCommand : ICommand
    {
        private readonly int _removedLine;

        private readonly List<Chip> _chips;

        private readonly int _score;


        public RemoveSingleLineCommand(List<Chip> chips, int line)
        {
            _chips = chips;

            _removedLine = line;

            //_score = GameData.GetScore(_removedLine);
        }


        public async UniTask Execute()
        {
            foreach (Chip chip in _chips)
            {
                chip.SetState(ChipState.Removed);
            }

            GameManager.Instance.AddScore(_score);

            await UniTask.Yield();
        }


        public async UniTask Undo()
        {
            await MoveChipsDownAsync(_removedLine);

            RestoreLine().Forget();

            GameManager.Instance.AddScore(-_score);
        }


        private async UniTask MoveChipsDownAsync(int lineNumber)
        {
            var chipsBelow = ChipRegistry.GetChipsOnLineAndBelow(lineNumber);

            List<UniTask> tasks = new();

            foreach (Chip chip in chipsBelow)
            {
                tasks.Add(chip.MoveDownAsync());
            }

            await UniTask.WhenAll(tasks);
        }


        private async UniTaskVoid RestoreLine()
        {
            Debug.Log($"Restore line ({_removedLine})");

            foreach (Chip chip in _chips)
            {
                ChipRegistry.RegisterInGame(chip);

                chip.SetState(ChipState.LightOff);

                chip.PlaceOnBoardAsync().Forget();
            }

            await UniTask.Yield();
        }
    }
}