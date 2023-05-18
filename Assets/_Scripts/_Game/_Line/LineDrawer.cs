using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LineDrawer : Singleton<LineDrawer>
{
    [SerializeField]
    private Line linePrefab;

    private const float PointStep = 0.05f;

    private const float WaveHeight = 0.15f;

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


    [Button("Draw Horizontal")]
    private void DrawRightLine(Vector3 startPoint, int length = 9)
    {
        Line line = Instantiate(linePrefab, startPoint, Quaternion.identity, transform);

        HashSet<Vector3> newPositions = new();

        for (int i = 0; i < length; i++)
        {
            foreach (Vector2 point in _baseValues)
            {
                float x = i + point.x;
                float y = startPoint.y + point.y;
                newPositions.Add(new Vector3(x, y, 0));
            }
        }

        line.SetPositions(newPositions.ToArray(), Color.blue, Color.cyan, 1f);
    }
}