using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public void Execute()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        RecordChips().Forget();
    }


    public void Undo()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        RestoreChips().Forget();
    }


    private async UniTaskVoid RecordChips()
    {
        _addedChips = await ChipController.Instance.CloneInGameChipsAsync();

        ChipController.Instance.ChipRegistry.CheckBoardCapacity();

        GameGUI.Instance.SetButtonPressPermission(true);
    }


    private async UniTaskVoid RestoreChips()
    {
        _addedChips.Reverse();

        await ChipController.Instance.RemoveChipsAsync(_addedChips);

        GameGUI.Instance.SetButtonPressPermission(true);
    }
}