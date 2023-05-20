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
    
    
    public static ChipData GetChipDataByChance()
    {
        // return Random.value <= _gameManager.ChanceForRandom
        //         ? GetRandomChipData()
        //         : GetChipDataForHardLevel();

        return new ChipData(0,0);
    }
    
    private ChipData GetRandomChipData()
    {
        int shapeIndex = _gameManager.gameData.GetRandomShapeIndex();

        int colorIndex = _gameManager.gameData.GetRandomColorIndex();

        return new ChipData(shapeIndex, colorIndex);
    }
    
    private ChipData GetChipDataForHardLevel()
    {
        List<int> shapeIndexes = new(_gameManager.gameData.GetShapeIndexes());
        List<int> colorIndexes = new(_gameManager.gameData.GetColorIndexes());

        if (_chipRegistry.Counter > 0)
        {
            shapeIndexes.Remove(_chipRegistry.InGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_chipRegistry.InGameChips.Last().ColorIndex);

            if (_chipRegistry.Counter >= _gameManager.gameData.width)
            {
                Chip chip = _chipRegistry.InGameChips[^_gameManager.gameData.width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }

}