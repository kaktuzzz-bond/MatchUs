using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RemoveSingleLineCommand : ICommand
{
    private readonly int _removedLine;

    private readonly List<Chip> _chips;

    private readonly int _score;

    ChipRegistry _registry;


    public RemoveSingleLineCommand(List<Chip> chips, int line, ChipRegistry registry)
    {
        _chips = chips;

        _removedLine = line;

        _registry = registry;

        //_score = GameData.GetScore(_removedLine);
    }


    public async UniTask Execute()
    {
        foreach (Chip chip in _chips)
        {
            chip.SetState(Chip.States.Removed);
        }

        GameManager.Instance.AddScore(_score);
    }


    public async UniTask Undo()
    {
        Board.Instance.RestoreLine(_removedLine);

        await RestoreLine();

        GameManager.Instance.AddScore(-_score);
    }


    private async UniTask RestoreLine()
    {
        List<UniTask> tasks = new();

        // foreach (ChipFiniteStateMachine state in _chipStates)
        // {
        //     Vector3 chipPos = state.transform.position;
        //
        //     chipPos.y = _removedLine;
        //
        //     state.transform.position = chipPos;
        //
        //     tasks.Add(state.SetRestoredState());
        // }

        await UniTask.WhenAll(tasks);
    }
}