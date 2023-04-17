using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : Singleton<Board>
{
    public event Action OnTilesGenerated;

    [HorizontalGroup("Size", Title = "Board Settings", Width = 0.5f)]
    [SerializeField, BoxGroup("Size/Width"), HideLabel, ReadOnly]
    private int width = 9;

    [SerializeField, BoxGroup("Size/Height"), HideLabel]
    private int height = 50;

    [HorizontalGroup("Prefabs", Width = 0.5f)]
    [SerializeField, BoxGroup("Prefabs/Tile Prefab"), HideLabel]
    private Transform tilePrefab;

    [SerializeField, BoxGroup("Prefabs/Dot Prefab"), HideLabel]
    private Transform dotPrefab;

    [SerializeField, FoldoutGroup("Parents")]
    private Transform tileParent;

    [ShowInInspector, FoldoutGroup("Parents")]
    public Transform ChipParent { get; private set; }

    [ShowInInspector, FoldoutGroup("Parents")]
    public Transform PointerParent { get; private set; }

    [SerializeField, FoldoutGroup("Chips"), ColorPalette]
    private Color[] colorPallet;

    [SerializeField, FoldoutGroup("Chips")]
    private Sprite[] shapePallet;

    public int Width => width;

    public int Height => height;


    [Button("Draw Board")]
    private void ButtonClicked() => DrawBoard(width, height);


    private Transform[,] _tiles;

    public Vector3 this[int x, int y] => _tiles[x, y].position;


    public Color GetColor(int index)
    {
        if (index < 0 ||
            index >= colorPallet.Length)
            throw new IndexOutOfRangeException($"{nameof(GetColor)}Index is out of range");

        return colorPallet[index];
    }


    public Color GetShape(int index)
    {
        if (index < 0 ||
            index >= shapePallet.Length)
            throw new IndexOutOfRangeException($"{nameof(GetShape)}Index is out of range");

        return colorPallet[index];
    }


    private void DrawBoard(int xSize, int ySize)
    {
        _tiles = new Transform[xSize, ySize];

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Transform tile = Instantiate(
                        original: tilePrefab,
                        position: new Vector3(x, -y, 0f),
                        rotation: Quaternion.identity,
                        parent: tileParent);

                tile.name = $"Tile({x}, {y})";

                DrawDot(x, y);
            }
        }

        OnTilesGenerated?.Invoke();
    }


    private void DrawDot(int x, int y)
    {
        if (Random.value > 0.15f ||
            x == x - 1 ||
            y == y - 1) return;

        Vector3 dotPos = new Vector3(x + 0.5f, -(y + 0.5f), 0f);

        Transform dot = Instantiate(
                original: dotPrefab,
                position: dotPos,
                rotation: Quaternion.identity,
                parent: tileParent);

        dot.name = "Dot";

        SpriteRenderer sr = dot.GetComponentInChildren<SpriteRenderer>();

        int index = Random.Range(0, colorPallet.Length);

        sr.color = GetColor(index);
    }
}