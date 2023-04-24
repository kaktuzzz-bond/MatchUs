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


    private void Awake()
    {
        _board = Board.Instance;

        _chipController = ChipController.Instance;

        _gameController = GameController.Instance;
    }


    private void Register(Chip chip)
    {
        _inGameChips.Add(chip);

        _chipCounter++;
    }


    public ChipData GetNewChipData()
    {
        return _gameController.DifficultyLevel switch
        {
                DifficultyLevel.Test => GetRandomChipData(1f),
                DifficultyLevel.Easy => GetRandomChipData(0.9f),
                DifficultyLevel.Normal => GetRandomChipData(0.6f),
                DifficultyLevel.Hard => GetRandomChipData(0.0f),
                _ => throw new ArgumentOutOfRangeException($"ERROR!")
        };
    }


    private ChipData GetRandomChipData(float chanceForRandomData)
    {
        return Random.value <= chanceForRandomData
                ? GetRandomChipData()
                : GetClearedChipData();
    }


    private ChipData GetRandomChipData()
    {
        int shapeIndex = Random.Range(0, _board.ShapePalletLength);

        int colorIndex = Random.Range(0, _board.ColorPalletLength);

        return new ChipData(shapeIndex, colorIndex);
    }


    private ChipData GetClearedChipData()
    {
        var shapeIndexes = Utils.GetIndexes(_board.ShapePalletLength);
        var colorIndexes = Utils.GetIndexes(_board.ColorPalletLength);

        if (_chipCounter > 0)
        {
            shapeIndexes.Remove(_inGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_inGameChips.Last().ColorIndex);

            if (_chipCounter >= _board.Width)
            {
                shapeIndexes.Remove(_inGameChips[^_board.Width].ShapeIndex);
                colorIndexes.Remove(_inGameChips[^_board.Width].ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }


#region Enable / Disable

    private void OnEnable()
    {
        _chipController.OnChipCreated += Register;
    }


    private void OnDisable()
    {
        _chipController.OnChipCreated -= Register;
    }

#endregion
}