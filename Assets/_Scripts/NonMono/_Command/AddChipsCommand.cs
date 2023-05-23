using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public async UniTask Execute()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        GameGUI.Instance.HideInfo();
        
        RecordChips().Forget();

        UniTask.Yield();
    }


    public async UniTask Undo()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        _addedChips.Reverse();

        //await ChipController.Instance.RemoveChipsAsync(_addedChips);

        GameGUI.Instance.SetButtonPressPermission(true);
    }


    private async UniTaskVoid RecordChips()
    {
        //_addedChips = await ChipController.Instance.CloneInGameChipsAsync();

        ChipController.Instance.Registry.CheckBoardCapacity();

        GameGUI.Instance.SetButtonPressPermission(true);
    }
}