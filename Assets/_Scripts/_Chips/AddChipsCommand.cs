using System;
using System.Collections.Generic;
using System.Linq;

public class AddChipsCommand : ICommand
{
    private readonly ChipController _chipController;


    public AddChipsCommand()
    {
        _chipController = ChipController.Instance;
    }


    public void Execute()
    {
        var inGameChips = _chipController.InGameChips
                .Where(c => c.StateManager.CurrentState.GetType() == typeof(FadedInChipState))
                .ToList();

        foreach (Chip chip in inGameChips)
        {
            _chipController.CreateChip(chip.ShapeIndex, chip.ColorIndex);
        }
    }


    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}