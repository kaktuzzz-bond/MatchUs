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

    private const float PointStep = 0.1f;

    private const float WaveHeight = 0.15f;

    private const float ShowTime = 0.4f;

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
            float y = Mathf.Sin(x * 2f * Mathf.PI) * WaveHeight;

            points.Add(new Vector2(x, y));
        }

        return points.ToArray();
    }


    [Button("Test Right")]
    private void TestRight()
    {
        CreateLineAsync(
                        new Vector3(4f, 0, 0),
                        Color.red,
                        Color.white,
                        3,
                        LineDirection.Right)
                .Forget();
    }


    [Button("Test Left")]
    private void TestLeft()
    {
        CreateLineAsync(
                        new Vector3(4f, 0, 0),
                        Color.red,
                        Color.white,
                        1,
                        LineDirection.Left)
                .Forget();
    }


    [Button("Test Up")]
    private void TestUp()
    {
        CreateLineAsync(
                        new Vector3(4f, 0, 0),
                        Color.red,
                        Color.white,
                        6,
                        LineDirection.Up)
                .Forget();
    }


    [Button("Test Down")]
    private void TestDown()
    {
        CreateLineAsync(
                        new Vector3(4f, 0, 0),
                        Color.red,
                        Color.white,
                        9,
                        LineDirection.Down)
                .Forget();
    }


    private async UniTask CreateLineAsync(
            Vector3 startPoint,
            Color startColor,
            Color endColor,
            int length,
            LineDirection direction)
    {
        Line line = Instantiate(linePrefab, startPoint, Quaternion.identity, transform);

        HashSet<Vector3> newPositions = new();

        for (int i = 0; i < length; i++)
        {
            foreach (Vector2 point in _baseValues)
            {
                Vector3 pos = GetCoords(direction, startPoint, point, i);

                newPositions.Add(pos);
            }
        }

        await line.DrawLineAsync(newPositions.ToArray(), startColor, endColor, ShowTime);
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