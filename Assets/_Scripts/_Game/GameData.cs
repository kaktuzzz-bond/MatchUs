using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New game data", menuName = "Create data")]
public class GameData : SerializedScriptableObject
{
    [ShowInInspector]
    public float CameraOrthographicSize { get; private set; }

    [TitleGroup("Board", GroupName = "Board Size")]
    [HorizontalGroup("Board/Size", 0.5f)]
    [BoxGroup("Board/Size/Width"), HideLabel]
    public int width;

    [BoxGroup("Board/Size/Height"), HideLabel]
    public int height;

#region PREFABS

    [Title("Assets Only Prefabs")]
    [AssetsOnly]
    public Transform tilePrefab;

    [AssetsOnly]
    public Transform dotPrefab;

    [AssetsOnly]
    public Transform chipPrefab;

    [AssetsOnly]
    public Transform hintPrefab;

    [AssetsOnly]
    public Transform selectorPrefab;

#endregion

    [Title("Chip data")]
    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public List<Sprite> shapes;

    [ColorPalette]
    public List<Color> colors;

#region DIFFICULTY SETTINGS

    [TitleGroup("Top", GroupName = "Game Mode Difficulty")]
    [BoxGroup("Top/Difficulty", ShowLabel = false)]
    [HorizontalGroup("Top/Difficulty/Split", 0.5f)]
    [BoxGroup("Top/Difficulty/Split/Chips Number"), LabelText("Easy")]
    public int easyMode;

    [BoxGroup("Top/Difficulty/Split/Random"), HideLabel, Range(0f, 1f)]
    public float easyRandom;

    [BoxGroup("Top/Difficulty/Split/Chips Number"), LabelText("Normal")]
    public int normalMode;

    [BoxGroup("Top/Difficulty/Split/Random"), HideLabel, Range(0f, 1f)]
    public float normalRandom;

    [BoxGroup("Top/Difficulty/Split/Chips Number"), LabelText("Hard")]
    public int hardMode;

    [BoxGroup("Top/Difficulty/Split/Random"), HideLabel, Range(0f, 1f)]
    public float hardRandom;

    [BoxGroup("Top/Difficulty/Split/Chips Number"), LabelText("Test")]
    public int testMode;

    [BoxGroup("Top/Difficulty/Split/Random"), HideLabel, Range(0f, 1f)]
    public float testRandom;

#endregion


    public IEnumerable<int> GetShapeIndexes()
    {
        return Utils.GetIndexes(shapes.Count);
    }


    public IEnumerable<int> GetColorIndexes()
    {
        return Utils.GetIndexes(colors.Count);
    }


    public Color GetColor(int index)
    {
        if (index < 0 ||
            index >= colors.Count)
        {
            throw new IndexOutOfRangeException($"{nameof(GetColor)}Index is out of range");
        }

        return colors[index];
    }


    public Sprite GetShape(int index)
    {
        if (index < 0 ||
            index >= shapes.Count)
        {
            throw new IndexOutOfRangeException($"{nameof(GetShape)}Index is out of range");
        }

        return shapes[index];
    }


    public Color GetRandomColor() => colors[GetRandomColorIndex()];


    public int GetRandomColorIndex() => Random.Range(0, colors.Count);


    public Sprite GetRandomShape() => shapes[GetRandomShapeIndex()];


    public int GetRandomShapeIndex() => Random.Range(0, shapes.Count);


    public void CalculateCameraOrthographicSize()
    {
        float aspectRatio = (float)Screen.height / Screen.width;

        CameraOrthographicSize = (width + 0.5f) * aspectRatio * 0.5f;
    }
}