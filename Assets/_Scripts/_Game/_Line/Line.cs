using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Line : MonoBehaviour
{
    private TrailRenderer _trailRenderer;


    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }


    public async UniTask SetPositions(Vector3[] positions, Color startColor, Color endColor, float showTime = 0.4f)
    {
        _trailRenderer.startColor = startColor;

        _trailRenderer.endColor = endColor;
        
        foreach (Vector3 pos in positions)
        {
            transform.position = pos;

            await UniTask.Yield();
        }

        _trailRenderer.time = showTime;

        await UniTask.Yield();
    }
}