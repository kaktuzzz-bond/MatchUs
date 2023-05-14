using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShuffleCommand : ICommand
{
    private Dictionary<Vector2Int, Chip> _original;


    public void Execute()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        SendChipsToNewPositions().Forget();
    }


    public void Undo()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        SendChipsToOriginalPositions().Forget();
    }


    private async UniTaskVoid SendChipsToNewPositions()
    {
        var inGameChips = ChipController.Instance.ChipRegistry.ActiveChips;

        _original = inGameChips.ToDictionary(chip => chip.BoardPosition);

        var modified = inGameChips.Shuffle();

        var tasks = Enumerable
                .Select(
                        inGameChips,
                        (t, i) => modified[i].MoveTo(t.BoardPosition))
                .ToList();

        await UniTask.WhenAll(tasks);

        GameGUI.Instance.SetButtonPressPermission(true);
    }


    private async UniTaskVoid SendChipsToOriginalPositions()
    {
        var tasks = Enumerable
                .Select(
                        _original,
                        pair => pair.Value.MoveTo(pair.Key))
                .ToList();

        await UniTask.WhenAll(tasks);

        GameGUI.Instance.SetButtonPressPermission(true);
    }
}