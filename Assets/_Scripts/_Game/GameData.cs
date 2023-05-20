using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New game data", menuName = "Create data")]
public class GameData : SerializedScriptableObject
{
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

    [Title("Chip data")]
    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public List<Sprite> shapes;

    [ColorPalette]
    public List<Color> colors;

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