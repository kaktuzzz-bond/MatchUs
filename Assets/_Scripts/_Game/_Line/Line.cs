using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    private LineRenderer _lineRenderer;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    public void SetPositions(Vector3[] positions)
    {
        Debug.Log($"SetPositions {positions.Length}");

        _lineRenderer.positionCount = positions.Length;

        _lineRenderer.SetPositions(positions);
        
        // _lineRenderer.Simplify(0.05f);
    }
}