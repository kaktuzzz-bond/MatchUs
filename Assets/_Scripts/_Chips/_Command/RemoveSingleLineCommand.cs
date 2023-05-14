using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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


    public async UniTask Undo()
    {
        Debug.LogWarning($"Restore line : ({_removedLine})");
        
        Board.Instance.RestoreLine(_removedLine);

        await RestoreLine();

        GameManager.Instance.AddScore(-_score);
    }


    private async UniTask RestoreLine()
    {
        List<UniTask> tasks = new();
        
        foreach (ChipFiniteStateMachine state in _chipStates)
        {
            Vector3 chipPos = state.transform.position;

            chipPos.y = _removedLine;

            state.transform.position = chipPos;

            tasks.Add(state.SetRestoredState());
        }

        await UniTask.WhenAll(tasks);
    }
}