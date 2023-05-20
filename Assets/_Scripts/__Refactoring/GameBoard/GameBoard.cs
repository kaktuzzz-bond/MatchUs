using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Capacity { get; private set; }

    private const float DotRandomizationValue = 0.15f;

    private readonly Vector3[,] _tilePositions;

    public List<Vector3> Dots { get; } = new();


    public GameBoard(int width, int height)
    {
        Width = width;
            
        Height = height;

        Capacity = width * height;
            
        _tilePositions = GetTilePositions(width, height);
    }


    public Vector3 this[int x, int y] => _tilePositions[x, y];


    private Vector3[,] GetTilePositions(int width, int height)
    {
        var positions = new Vector3[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                positions[x, y] = new Vector3(x, -y, 0f);

                DrawDot(x, y);
            }
        }

        return positions;
    }


    private void DrawDot(int x, int y)
    {
        if (x == Width - 1 ||
            y == Height - 1 ||
            Random.value > DotRandomizationValue)
        {
            return;
        }

        float dotX = x + 0.5f;

        float dotY = -(y + 0.5f);

        Dots.Add(new Vector3(dotX, dotY, 0));
    }
}