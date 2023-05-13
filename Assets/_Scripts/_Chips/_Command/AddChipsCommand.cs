using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public void Execute()
    {
        RecordAdded().Forget();

        ChipRegistry.Instance.CheckBoardCapacity();
    }


    public void Undo()
    {
        _addedChips.Reverse();

        ChipController.Instance.RemoveChips(_addedChips);
    }


    private async UniTask RecordAdded() =>
            _addedChips = await ChipController.Instance.CloneInGameChipsAsync();
}