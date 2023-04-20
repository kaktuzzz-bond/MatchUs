using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameUIController : Singleton<GameUIController>
{
    [SerializeField]
    private RectTransform header;


    public Vector3[] GetHeaderCorners() => Utils.GetRectWorldCorners(header);
}