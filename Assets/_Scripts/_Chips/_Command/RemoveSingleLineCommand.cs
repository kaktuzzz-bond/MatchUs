using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemoveSingleLineCommand : ICommand
{
    private readonly int _removedLine;

    private readonly List<ChipFiniteStateMachine> _chipStates;

    private readonly int _score;


    public RemoveSingleLineCommand(List<ChipFiniteStateMachine> chipStates)
    {
        _chipStates = chipStates;

        _removedLine = _chipStates.First().Chip.BoardPosition.y;

        _score = GameConfig.GetScore(_removedLine);
    }


    public void Execute()
    {
        Debug.Log($"Removed single line in ({_removedLine})");

        ChipFiniteStateMachine.DisableChips(_chipStates);

        GameManager.Instance.AddScore(_score);
    }


    public void Undo()
    {
        Board.Instance.RestoreLine(_removedLine);

        foreach (ChipFiniteStateMachine state in _chipStates)
        {
            Vector3 chipPos = state.transform.position;

            chipPos.y = _removedLine;

            state.transform.position = chipPos;

            state.SetRestoredState();
        }

        GameManager.Instance.AddScore(-_score);
    }
}