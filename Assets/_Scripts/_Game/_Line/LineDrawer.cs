using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class LineDrawer : Singleton<LineDrawer>
{
    public enum LineDirection
    {
        Right,

        Left,

        Up,

        Down
    }

    [SerializeField]
    private Line linePrefab;

    private const float PointStep = 0.05f;

    private const float WaveHeight = 0.15f;

    private const float ShowTime = 0.8f;

    private Vector2[] _baseValues;


    private void Start()
    {
        _baseValues = GetBaseArray();
    }


    private Vector2[] GetBaseArray()
    {
        List<Vector2> points = new();

        for (float x = 0f; x <= 1f; x += PointStep)
        {
            float y = Mathf.Sin(x * 2 * Mathf.PI) * WaveHeight;

            points.Add(new Vector2(x, y));
        }

        return points.ToArray();
    }


    [Button("Test Right")]
    private void TestRight()
    {
        var positions = GetLinePoints(new Vector3(4f, 0, 0), 4, LineDirection.Right);

        CreateLineAsync(positions, Color.red, Color.white).Forget();
    }


    [Button("Test Left")]
    private void TestLeft()
    {
        var positions = GetLinePoints(new Vector3(4f, 0, 0), 1, LineDirection.Left);

        CreateLineAsync(positions, Color.red, Color.white).Forget();
    }


    [Button("Test Up")]
    private void TestUp()
    {
        var positions = GetLinePoints(new Vector3(4f, 0, 0), 6, LineDirection.Up);

        CreateLineAsync(positions, Color.red, Color.white).Forget();
    }


    [Button("Test Down")]
    private void TestDown()
    {
        var positions = GetLinePoints(new Vector3(4f, 0, 0), 9, LineDirection.Down);

        CreateLineAsync(positions, Color.red, Color.white).Forget();
    }


    public async UniTask CreateLineAsync(Vector3[] positions, Color startColor, Color endColor)
    {
        if (positions == null ||
            positions.Length == 0) return;

        Line line = Instantiate(linePrefab, positions[0], Quaternion.identity, transform);

        await line.DrawLineAsync(positions, startColor, endColor, ShowTime);
    }


    // public Vector3[] GetLinePoints(Vector2Int startBoardPoint, int length, LineDirection direction)
    // {
    //     Vector3 startPoint = Board.Instance[startBoardPoint.x, startBoardPoint.y].position;
    //
    //     return GetLinePoints(startPoint, length, direction);
    // }


    private Vector3[] GetLinePoints(Vector3 startPoint, int length, LineDirection direction)
    {
        HashSet<Vector3> newPositions = new();

        for (int i = 0; i < length; i++)
        {
            foreach (Vector2 point in _baseValues)
            {
                Vector3 pos = GetCoords(direction, startPoint, point, i);

                newPositions.Add(pos);
            }
        }

        return newPositions.ToArray();
    }


    private Vector3 GetCoords(LineDirection direction, Vector3 startPoint, Vector2 basePoint, int step)
    {
        Vector3 coord = new();

        switch (direction)
        {
            case LineDirection.Right:

                coord.x = startPoint.x + basePoint.x + step;

                coord.y = startPoint.y + basePoint.y;

                return coord;

            case LineDirection.Left:

                coord.x = startPoint.x - (basePoint.x + step);

                coord.y = startPoint.y + basePoint.y;

                return coord;

            case LineDirection.Up:

                coord.x = startPoint.x + basePoint.y;

                coord.y = startPoint.y + basePoint.x + step;

                return coord;

            case LineDirection.Down:

                coord.x = startPoint.x + basePoint.y;

                coord.y = startPoint.y - (basePoint.x + step);

                return coord;

            default:

                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}