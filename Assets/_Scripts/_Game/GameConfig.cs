using System;
using UnityEngine;

public static class GameConfig
{
    public static int GetChipsOnStart(DifficultyLevel difficultyLevel)
    {
        return difficultyLevel switch
        {
                DifficultyLevel.Test => 9,
                DifficultyLevel.Easy => 27,
                DifficultyLevel.Normal => 27,
                DifficultyLevel.Hard => 45,
                _ => throw new ArgumentOutOfRangeException()
        };
    }


    public static float GetChanceForRandom(DifficultyLevel difficultyLevel)
    {
        return difficultyLevel switch
        {
                DifficultyLevel.Test => 1.0f,
                DifficultyLevel.Easy => 0.8f,
                DifficultyLevel.Normal => 0.6f,
                DifficultyLevel.Hard => 0.3f,
                _ => throw new ArgumentOutOfRangeException()
        };
    }


    public static int GetScore(Chip first, Chip second)
    {
        int score = (first.BoardPosition.y + 1) + (second.BoardPosition.y + 1);

        if (first.CompareShape(second) &&
            first.CompareColor(second))
        {
            score *= 2;
        }

        return score;
    }


    public static int GetScore(int boardLine) => (boardLine + 1) * Board.Instance.Width;
}