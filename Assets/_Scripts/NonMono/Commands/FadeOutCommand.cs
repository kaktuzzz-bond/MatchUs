using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;

namespace NonMono.Commands
{
    public class FadeOutCommand : ICommand
    {
        private readonly Chip _first;

        private readonly Chip _second;

        private readonly int _score;


        public FadeOutCommand(Chip first, Chip second)
        {
            _first = first;
            _second = second;

            //_score = GameData.GetScore(first, second);
        }


        public async UniTask Execute()
        {
            _first.SetState(ChipState.LightOff);

            _second.SetState(ChipState.LightOff);

            var emptyLines = LineChecker.GetEmptyLines(_first, _second);

            if (emptyLines.Count == 0) return;

            RemoveLines(emptyLines).Forget();
        
        
            GameManager.Instance.AddScore(_score);

            await UniTask.Yield();
        }


        public async UniTask Undo()
        {
            _first.SetState(ChipState.LightOn);

            _second.SetState(ChipState.LightOn);

            CameraController.Instance.MoveToBoardPosition(_second.BoardPosition.y);

            GameManager.Instance.AddScore(-_score);

            await UniTask.Yield();
        }
    
        private async UniTaskVoid RemoveLines(List<List<Chip>> lines)
        {
            foreach (var list in lines)
            {
                int lineNumber = list.First().BoardPosition.y;

                await CommandLogger.AddCommand(new RemoveSingleLineCommand(list, lineNumber));

                await MoveChipsUpAsync(lineNumber);

                CameraController.Instance.MoveToBottomBound();

                ChipRegistry.CheckBoardCapacity();
            }
        }


        private async UniTask MoveChipsUpAsync(int lineNumber)
        {
            var chipsBelow = ChipRegistry.GetChipsBelowLine(lineNumber);

            List<UniTask> tasks = new();

            foreach (Chip chip in chipsBelow)
            {
                tasks.Add(chip.MoveUpAsync());
            }

            await UniTask.WhenAll(tasks);
        }
    }
}