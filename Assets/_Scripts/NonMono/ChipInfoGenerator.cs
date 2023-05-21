using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipInfoGenerator
{
    private readonly List<ChipInfo> _chips = new();


    public List<ChipInfo> GetStartChipInfoArray()
    {
        _chips.Clear();

        int chipsOnStart = GameManager.Instance.gameData.GetOnStartChipNumber();
        float randomizer = GameManager.Instance.gameData.GetRandomizeValue();

        for (int i = 0; i < chipsOnStart; i++)
        {
            ChipInfo info = GetChipDataByChance(randomizer);

            _chips.Add(info);
        }

        return _chips;
    }


    public List<ChipInfo> GetClonedInfo(List<ChipInfo> origin, int counter)
    {
        var cloned = new List<ChipInfo>(origin);

        for (int i = 0; i < origin.Count; i++)
        {
            Vector2Int boardPos = GetBoardPosition(counter + i);

            cloned[i].position = Utils.ConvertBoardToWorldCoordinates(boardPos);
        }

        return cloned;
    }

    
    public List<ChipInfo> ExtractInfos(List<Chip> origin)
    {
        return origin.Select(chip => chip.GetInfo()).ToList();
    }
    
    // public async UniTask<List<Chip>> CloneInGameChipsAsync()
    // {
    //     List<Chip> added = new();
    //
    //     var chips = ChipRegistry.ActiveChips;
    //
    //     int line = NextBoardPosition.y;
    //
    //     foreach (Chip chip in chips)
    //     {
    //         Vector2Int boardPos = NextBoardPosition;
    //
    //         if (NextLine(ref line, boardPos.y))
    //         {
    //             await UniTask.Delay(1000);
    //
    //             CameraController.Instance.MoveToBottomBound();
    //         }
    //
    //         //Chip newChip = DrawChip(chip.ShapeIndex, chip.ColorIndex, boardPos);
    //
    //         //added.Add(newChip);
    //     }
    //
    //     return added;
    // }
    //
    //
    // private bool NextLine(ref int line, int boardPosY)
    // {
    //     if (boardPosY == line) return false;
    //
    //     line = boardPosY;
    //
    //     return true;
    // }


    private ChipInfo GetChipDataByChance(float chance)
    {
        return Random.value <= chance
                ? GetRandomChipData()
                : GetChipDataForHardLevel();
    }


    private ChipInfo GetRandomChipData()
    {
        ChipInfo info = new()
        {
                shapeIndex = GameManager.Instance.gameData.GetRandomShapeIndex(),
                colorIndex = GameManager.Instance.gameData.GetRandomColorIndex(),
                state = Chip.States.LightOn
        };

        Vector2Int boardPos = GetBoardPosition(_chips.Count);

        info.position = Utils.ConvertBoardToWorldCoordinates(boardPos);

        return info;
    }


    private ChipInfo GetChipDataForHardLevel()
    {
        List<int> shapeIndexes = new(GameManager.Instance.gameData.GetShapeIndexes());
        List<int> colorIndexes = new(GameManager.Instance.gameData.GetColorIndexes());

        if (_chips.Count > 0)
        {
            shapeIndexes.Remove(_chips.Last().shapeIndex);
            colorIndexes.Remove(_chips.Last().colorIndex);

            if (_chips.Count >= GameManager.Instance.gameData.width)
            {
                ChipInfo chipInfo = _chips[^GameManager.Instance.gameData.width];

                shapeIndexes.Remove(chipInfo.shapeIndex);
                colorIndexes.Remove(chipInfo.colorIndex);
            }
        }

        ChipInfo info = new()
        {
                shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)],
                colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)],
                state = Chip.States.LightOn
        };

        Vector2Int boardPos = GetBoardPosition(_chips.Count);

        info.position = Utils.ConvertBoardToWorldCoordinates(boardPos);

        return info;
    }


    private Vector2Int GetBoardPosition(int count)
    {
        return new Vector2Int(
                count % GameManager.Instance.gameData.width,
                count / GameManager.Instance.gameData.width);
    }
}