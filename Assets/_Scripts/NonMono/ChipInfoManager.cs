using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChipInfoManager
{
    private static readonly List<ChipInfo> Infos = new();


    public static List<ChipInfo> GetStartChipInfoArray()
    {
        Infos.Clear();

        int chipsOnStart = GameManager.Instance.gameData.GetOnStartChipNumber();
        float randomizer = GameManager.Instance.gameData.GetRandomizeValue();

        for (int i = 0; i < chipsOnStart; i++)
        {
            ChipInfo info = GetChipDataByChance(randomizer);

            Infos.Add(info);
        }

        return Infos;
    }


    public static List<ChipInfo> GetClonedInfo(List<ChipInfo> origin, int counter)
    {
        List<ChipInfo> chipInfos = new();

        for (int i = 0; i < origin.Count; i++)
        {
            Vector2Int boardPos = Board.GetBoardPosition(counter + i);

            ChipInfo newInfo = new (
                    shapeIndex: origin[i].shapeIndex,
                    colorIndex: origin[i].colorIndex,
                    position: Utils.ConvertBoardToWorldCoordinates(boardPos),
                    state: origin[i].state);

            chipInfos.Add(newInfo);
        }

        return chipInfos;
    }


    public static List<ChipInfo> ExtractInfos(List<Chip> origin)
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


    private static ChipInfo GetChipDataByChance(float chance)
    {
        return Random.value <= chance
                ? GetRandomChipData()
                : GetChipDataForHardLevel();
    }


    private static ChipInfo GetRandomChipData()
    {
        ChipInfo info = new()
        {
                shapeIndex = GameManager.Instance.gameData.GetRandomShapeIndex(),
                colorIndex = GameManager.Instance.gameData.GetRandomColorIndex(),
                state = Chip.States.LightOn
        };

        Vector2Int boardPos = Board.GetBoardPosition(Infos.Count);

        info.position = Utils.ConvertBoardToWorldCoordinates(boardPos);

        return info;
    }


    private static ChipInfo GetChipDataForHardLevel()
    {
        List<int> shapeIndexes = new(GameManager.Instance.gameData.GetShapeIndexes());
        List<int> colorIndexes = new(GameManager.Instance.gameData.GetColorIndexes());

        if (Infos.Count > 0)
        {
            shapeIndexes.Remove(Infos.Last().shapeIndex);
            colorIndexes.Remove(Infos.Last().colorIndex);

            if (Infos.Count >= GameManager.Instance.gameData.width)
            {
                ChipInfo chipInfo = Infos[^GameManager.Instance.gameData.width];

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

        Vector2Int boardPos = Board.GetBoardPosition(Infos.Count);

        info.position = Utils.ConvertBoardToWorldCoordinates(boardPos);

        return info;
    }
}