using System.Collections.Generic;
using UnityEngine;

public class AddChipsCommand : ICommand
{
    private List<Chip> _addedChips = new();


    public AddChipsCommand()
    {
        ChipController.Instance.OnChipsAdded += RecordAdded;
    }


    public void Execute()
    {
        ChipController.Instance.CloneInGameChips();

        ChipController.Instance.Log.AddCommand(this);

        ChipRegistry.Instance.CheckBoardCapacity();
    }


    public void Undo()
    {
        
        foreach (Chip chip in _addedChips)
        {
            chip.ChipFiniteStateMachine.SetSelfDestroyableState();
        }

        ChipRegistry.Instance.CheckBoardCapacity();

        //_addedChips.Clear();
    }


    private void RecordAdded(List<Chip> added)
    {
        Debug.LogError($"Record: ({added.Count})");

        _addedChips = added;
        
        ChipController.Instance.OnChipsAdded -= RecordAdded;
    }
}