using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShuffleCommand : ICommand
{
    private Dictionary<Vector2Int, Chip> _original;


    public void Execute()
    {
        var inGameChips = ChipRegistry.Instance.ActiveChips;

        _original = inGameChips.ToDictionary(chip => chip.BoardPosition);

        var modified = inGameChips.Shuffle();

        SendChipsToNewPositions(inGameChips, modified);

        ChipController.Instance.Log.AddCommand(this);
    }


    public void Undo()
    {
        foreach (var pair in _original)
        {
           pair.Value.MoveTo(pair.Key);
        }
    }


    private static void SendChipsToNewPositions(IReadOnlyList<Chip> original, IReadOnlyList<Chip> modified)
    {
        for (int i = 0; i < original.Count; i++)
        {
            modified[i].MoveTo(original[i].BoardPosition);
        }
    }
}