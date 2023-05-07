using System.Collections.Generic;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public void Execute()
    {
        var inGameChips = ChipRegistry.Instance.ActiveChips;

        ChipController.Instance.CloneInGameChips(inGameChips, out _addedChips);

        ChipController.Instance.Log.AddCommand(this);

        CameraController.Instance.MoveToBottomBound();
    }


    public void Undo()
    {
        foreach (Chip chip in _addedChips)
        {
            chip.ChipFiniteStateMachine.SetSelfDestroyableState();
        }

        _addedChips.Clear();

        CameraController.Instance.MoveToBottomBound();
    }
}