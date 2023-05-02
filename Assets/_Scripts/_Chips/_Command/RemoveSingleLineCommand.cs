using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemoveSingleLineCommand : ICommand
{
    private readonly List<ChipStateManager> _chipStates;


    public RemoveSingleLineCommand( List<ChipStateManager> chipStates)
    {
        _chipStates = chipStates;
    }


    public void Execute()
    {
        Debug.Log($"Removed single line in ({_chipStates.First().Chip.BoardPosition.y})");
        
        ChipStateManager.DisableChips(_chipStates);
    }


    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}