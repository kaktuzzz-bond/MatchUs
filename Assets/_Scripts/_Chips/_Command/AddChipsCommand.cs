using System;
using System.Collections.Generic;

public class AddChipsCommand : ICommand
{
    private readonly ChipController _chipController;

    private List<Chip> _addedChips;


    public AddChipsCommand()
    {
        _chipController = ChipController.Instance;
    }


    public void Execute()
    {
        var inGameChips = ChipRegistry.Instance.ActiveChips;

        _chipController.CloneInGameChips(inGameChips, out _addedChips);

        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        foreach (Chip chip in _addedChips)
        {
            chip.ChipFiniteStateMachine.SetSelfDestroyableState();
        }

        _addedChips.Clear();
    }
}