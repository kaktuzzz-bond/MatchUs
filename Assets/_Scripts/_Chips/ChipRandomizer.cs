using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipRandomizer
{
    private readonly Board _board;
    
    private readonly ChipRegistry _chipRegistry;
    
    private readonly GameManager _gameManager;
    public ChipRandomizer(ChipRegistry chipRegistry, Board board)
    {
        _gameManager = GameManager.Instance;
        
        _board = board;
        
        _chipRegistry = chipRegistry;
    }
    
    
    public ChipData GetChipDataByChance()
    {
        return Random.value <= _gameManager.ChanceForRandom
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
        List<int> shapeIndexes = new(_board.ShapeIndexes);
        List<int> colorIndexes = new(_board.ColorIndexes);

        if (_chipRegistry.Counter > 0)
        {
            shapeIndexes.Remove(_chipRegistry.InGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_chipRegistry.InGameChips.Last().ColorIndex);

            if (_chipRegistry.Counter >= _board.Width)
            {
                Chip chip = _chipRegistry.InGameChips[^_board.Width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }

}