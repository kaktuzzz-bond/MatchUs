using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShuffleCommand : ICommand
{
    private Dictionary<Vector2Int, Chip> _original;


    public async UniTask Execute()
    {
        ChipComparer.ClearStorage();
        
        GameGUI.Instance.SetButtonPressPermission(false);

        GameGUI.Instance.HideInfo();
                
        var inGameChips = ChipRegistry.ActiveChips;

        _original = inGameChips.ToDictionary(chip => chip.BoardPosition);

        var modified = inGameChips.Shuffle();

        var tasks = Enumerable
                .Select(
                        inGameChips,
                        (t, i) => modified[i].MoveToAsync(t.BoardPosition))
                .ToList();

        await UniTask.WhenAll(tasks);

        GameGUI.Instance.SetButtonPressPermission(true);
    }


    public async UniTask Undo()
    {
        GameGUI.Instance.SetButtonPressPermission(false);

        var tasks = Enumerable
                .Select(
                        _original,
                        pair => pair.Value.MoveToAsync(pair.Key))
                .ToList();

        await UniTask.WhenAll(tasks);

        GameGUI.Instance.SetButtonPressPermission(true);
    }

    
}