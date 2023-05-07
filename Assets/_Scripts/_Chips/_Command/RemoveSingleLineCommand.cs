#define ENABLE_LOGS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemoveSingleLineCommand : ICommand
{
    private readonly int _removedLine;

    private readonly List<ChipFiniteStateMachine> _chipStates;


    public RemoveSingleLineCommand(List<ChipFiniteStateMachine> chipStates)
    {
        _chipStates = chipStates;

        _removedLine = _chipStates.First().Chip.BoardPosition.y;
    }


    public void Execute()
    {
        Logger.Debug($"Removed single line in ({_removedLine})");

        ChipFiniteStateMachine.DisableChips(_chipStates);

        ChipController.Instance.Log.AddCommand(this);

        CameraController.Instance.MoveToBottomBound();
    }


    public void Undo()
    {
        ChipController.Instance.RestoreLine(_removedLine);

        foreach (ChipFiniteStateMachine state in _chipStates)
        {
            Vector3 chipPos = state.transform.position;

            chipPos.y = _removedLine;

            state.transform.position = chipPos;

            state.SetFadedOutState();

            state.SetEnabledState();
        }

        CameraController.Instance.MoveToBottomBound();
    }
}