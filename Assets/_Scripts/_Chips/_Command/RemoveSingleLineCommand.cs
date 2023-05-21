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

        //_removedLine = _chipStates.First()._chip.BoardPosition.y;

        //_score = GameData.GetScore(_removedLine);
    }


    public void Execute()
    {
        //ChipFiniteStateMachine.DisableChips(_chipStates);

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