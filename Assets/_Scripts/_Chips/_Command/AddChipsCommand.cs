using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public void Execute()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        GameGUI.Instance.HideInfo();
        
        RecordChips().Forget();
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

        ChipController.Instance.ChipRegistry.CheckBoardCapacity();

        GameGUI.Instance.SetButtonPressPermission(true);
    }
}