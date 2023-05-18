using System;
using System.Collections.Generic;
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


    public async UniTask SetPositions(Vector3[] positions)
    {
        foreach (Vector3 pos in positions)
        {
            transform.position = pos;

            await UniTask.Yield();
        }
    }
}