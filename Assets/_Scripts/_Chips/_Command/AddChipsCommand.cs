using System.Collections.Generic;
using UnityEngine;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips;


    public AddChipsCommand()
    {
        ChipController.Instance.OnChipsAdded += RecordAdded;
    }


    public void Execute()
    {
        ChipController.Instance.CloneInGameChips();

        ChipController.Instance.Log.AddCommand(this);
    }


    public void Undo()
    {
        _addedChips.Reverse();
        
        ChipController.Instance.RemoveChips(_addedChips);
    }


    private void RecordAdded(List<Chip> added)
    {
        _addedChips = new List<Chip>(added);

        ChipRegistry.Instance.CheckBoardCapacity();

        ChipController.Instance.OnChipsAdded -= RecordAdded;
    }
}