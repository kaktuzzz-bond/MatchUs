using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemoveSingleLineCommand : ICommand
{
    private readonly int _removedLine;

    private readonly List<ChipStateManager> _chipStates;

    private readonly ChipController _chipController;


    public RemoveSingleLineCommand(List<ChipStateManager> chipStates)
    {
        _chipStates = chipStates;

        _chipController = ChipController.Instance;

        _removedLine = _chipStates.First().Chip.BoardPosition.y;
    }


    public void Execute()
    {
        Debug.Log($"Removed single line in ({_removedLine})");

        ChipStateManager.DisableChips(_chipStates);

        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        _chipController.RestoreLine(_removedLine);
        
        foreach (ChipStateManager state in _chipStates)
        {
            state.transform.position -= new Vector3(0, 0.5f, 0);

            state.SetFadedOutState();

            state.SetEnabledState();
        }
    }
}