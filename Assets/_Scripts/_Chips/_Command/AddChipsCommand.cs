using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AddChipsCommand : ICommand
{
    private readonly ChipController _chipController;

    private readonly ChipRegistry _chipRegistry;
    
    private List<Chip> _addedChips;


    public AddChipsCommand()
    {
        _chipController = ChipController.Instance;
        _chipRegistry = ChipRegistry.Instance;
    }


    public void Execute()
    {
        var inGameChips = _chipRegistry.InGameChips
                .Where(c => c.ChipStateManager.CurrentState.GetType() == typeof(FadedInChipState))
                .ToList();

        
        _chipController.CloneInGameChips(inGameChips, out _addedChips);

        _chipController.Log.Push(this);
    }


    public void Undo()
    {
        foreach (Chip chip in _addedChips)
        {
            chip.ChipStateManager.SetSelfDestroyableState();
        }
        
        _addedChips.Clear();
    }
}