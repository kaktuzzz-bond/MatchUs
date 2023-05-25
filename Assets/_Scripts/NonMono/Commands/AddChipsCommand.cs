using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game;
using UI;
using UnityEngine;

namespace NonMono.Commands
{
    public class AddChipsCommand : ICommand
    {
        private readonly List<Chip> _addedChips = new();

        private readonly List<ChipInfo> _infos;


        public AddChipsCommand(List<ChipInfo> infos)
        {
            _infos = infos;
        }


        public async UniTask Execute()
        {
            GameGUI.Instance.SetButtonPressPermission(false);

            ChipComparer.ClearStorage();

            GameGUI.Instance.HideInfo();

            await DrawArrayAsync(_infos);

            ChipRegistry.CheckBoardCapacity();

            GameGUI.Instance.SetButtonPressPermission(true);
        }


        public async UniTask Undo()
        {
            GameGUI.Instance.SetButtonPressPermission(false);

            _addedChips.Reverse();

            await RemoveArrayAsync();

            ChipRegistry.CheckBoardCapacity();
            
            GameGUI.Instance.SetButtonPressPermission(true);
        }


        private async UniTask RemoveArrayAsync()
        {
            int line = _addedChips.First().BoardPosition.y;

            foreach (Chip chip in _addedChips)
            {
                if (chip.BoardPosition.y != line)
                {
                    line = chip.BoardPosition.y;

                    CameraController.Instance.MoveToBottomBound();
                
                    await UniTask.Delay(100);
                }

                chip.SetState(ChipState.Removed);

                ChipRegistry.UnregisterAndDestroy(chip).Forget();
            }
        }


        private async UniTask DrawArrayAsync(List<ChipInfo> chipInfos)
        {
            int line = (int)chipInfos.First().position.y;

            foreach (ChipInfo info in chipInfos)
            {
                if ((int)info.position.y != line)
                {
                    line = (int)info.position.y;

                    CameraController.Instance.MoveToBottomBound();
                
                    await UniTask.Delay(100);
                }

                Chip chip = CreateChip(info);

                _addedChips.Add(chip);

                ChipRegistry.RegisterInGame(chip);

                chip.Init(info);

                chip.PlaceOnBoardAsync().Forget();
            }
        }


        private Chip CreateChip(ChipInfo info)
        {
            Transform instance = Object.Instantiate(
                    GameManager.Instance.gameData.chipPrefab,
                    info.position,
                    Quaternion.identity,
                    GameManager.Instance.gameData.chipParent);

            if (!instance.TryGetComponent(out Chip chip)) return null;

            instance.name = $"Chip ({info.shapeIndex}, {info.colorIndex})";

            return chip;
        }
    }
}