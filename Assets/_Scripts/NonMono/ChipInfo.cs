using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NonMono
{
    [Serializable]
    public class ChipInfo
    {
        public int shapeIndex;

        public int colorIndex;

        public Vector3 position;

        public ChipState state;

        private static readonly List<ChipInfo> Infos = new();


        public ChipInfo(int shapeIndex, int colorIndex, Vector3 position, ChipState state)
        {
            this.shapeIndex = shapeIndex;
            this.colorIndex = colorIndex;
            this.position = position;
            this.state = state;
        }


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
                Vector2Int boardPos = Board.Board.GetBoardPosition(counter + i);

                ChipInfo newInfo = new(
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


        private static ChipInfo GetChipDataByChance(float chance)
        {
            return Random.value <= chance
                    ? GetRandomChipData()
                    : GetChipDataForHardLevel();
        }


        private static ChipInfo GetRandomChipData()
        {
            Vector2Int boardPos = Board.Board.GetBoardPosition(Infos.Count);

            ChipInfo info = new(
                    shapeIndex: GameManager.Instance.gameData.GetRandomShapeIndex(),
                    colorIndex: GameManager.Instance.gameData.GetRandomColorIndex(),
                    position: Utils.ConvertBoardToWorldCoordinates(boardPos),
                    state: ChipState.LightOn);

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

            Vector2Int boardPos = Board.Board.GetBoardPosition(Infos.Count);

            ChipInfo info = new(
                    shapeIndex: shapeIndexes[Random.Range(0, shapeIndexes.Count)],
                    colorIndex: colorIndexes[Random.Range(0, colorIndexes.Count)],
                    position: Utils.ConvertBoardToWorldCoordinates(boardPos),
                    state: ChipState.LightOn);

            return info;
        }
    }
}