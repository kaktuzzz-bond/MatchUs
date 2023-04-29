using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChipHandler : Singleton<ChipHandler>
{
    public Vector2Int NextBoardPosition =>
            new(_chipCounter % _board.Width, _chipCounter / _board.Width);

    private ChipController _chipController;

    private GameController _gameController;

    private Board _board;

    private int _chipCounter;

    [ShowInInspector]
    private List<Chip> _inGameChips = new();

    public List<Chip> InGameChips => _inGameChips;

    [ShowInInspector]
    private List<Chip> _outOfGameChips = new();

    private readonly Stack<ICommand> _log = new();


    private void Awake()
    {
        _board = Board.Instance;

        _chipController = ChipController.Instance;

        _gameController = GameController.Instance;
    }


    public void Execute(ICommand command)
    {
        command.Execute();

        _log.Push(command);
    }


    public void Undo()
    {
        ICommand command = _log.Pop();
        command.Undo();
    }


    public void Register(Chip chip)
    {
        _inGameChips.Add(chip);

        _outOfGameChips.Remove(chip);

        _chipCounter++;
    }


    public void Unregister(Chip chip)
    {
        _outOfGameChips.Add(chip);

        _inGameChips.Remove(chip);

        _chipCounter--;
    }


    public ChipData GetNewChipData()
    {
        return _gameController.DifficultyLevel switch
        {
                DifficultyLevel.Test => GetChipDataByChance(1.0f),
                DifficultyLevel.Easy => GetChipDataByChance(0.9f),
                DifficultyLevel.Normal => GetChipDataByChance(0.6f),
                DifficultyLevel.Hard => GetChipDataByChance(1f),
                _ => throw new ArgumentOutOfRangeException($"ERROR!")
        };
    }


    private ChipData GetChipDataByChance(float chanceForRandomData)
    {
        return Random.value <= chanceForRandomData
                ? GetRandomChipData()
                : GetChipDataForHardLevel();
    }


    private ChipData GetRandomChipData()
    {
        int shapeIndex = Random.Range(0, _board.ShapePalletLength);

        int colorIndex = Random.Range(0, _board.ColorPalletLength);

        return new ChipData(shapeIndex, colorIndex);
    }


    private ChipData GetChipDataForHardLevel()
    {
        var shapeIndexes = Utils.GetIndexes(_board.ShapePalletLength);
        var colorIndexes = Utils.GetIndexes(_board.ColorPalletLength);

        if (_chipCounter > 0)
        {
            shapeIndexes.Remove(_inGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_inGameChips.Last().ColorIndex);

            if (_chipCounter >= _board.Width)
            {
                Chip chip = _inGameChips[^_board.Width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }
}