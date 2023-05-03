using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : Singleton<Board>
{
    public event Action OnTilesGenerated;

    public event Action<int> OnLineRemoved;
    
    [HorizontalGroup("Size", Title = "Board Settings")]
    [SerializeField] [BoxGroup("Size/Width")] [HideLabel] [ReadOnly]
    private int width = 9;

    [SerializeField] [BoxGroup("Size/Height")] [HideLabel]
    private int height = 50;

    [SerializeField] [FoldoutGroup("Prefabs")]
    private Transform tilePrefab;

    [SerializeField] [FoldoutGroup("Prefabs")]
    private Transform dotPrefab;

    [SerializeField] [FoldoutGroup("Parents")]
    private Transform tileParent;

    [FoldoutGroup("Parents")]
    public Transform chipParent;

    [FoldoutGroup("Parents")]
    public Transform pointerParent;

    [SerializeField] [FoldoutGroup("Chips")] [ColorPalette]
    private Color[] colorPallet;

    [SerializeField] [FoldoutGroup("Chips")]
    private Sprite[] shapePallet;

    public int Width => width;

    public int Height => height;

    public List<int> ShapeIndexes { get; private set; }

    public List<int> ColorIndexes { get; private set; }

    public int ColorPalletLength => colorPallet.Length;

    public int ShapePalletLength => shapePallet.Length;

    private Transform[,] _tiles;

    public Transform this[int x, int y] => _tiles[x, y];


    private void Start()
    {
        ShapeIndexes = Utils.GetIndexes(ShapePalletLength);
        ColorIndexes = Utils.GetIndexes(ColorPalletLength);

        DrawBoard(Width, Height);
    }


    public Color GetColor(int index)
    {
        if (index < 0 ||
            index >= colorPallet.Length)
        {
            throw new IndexOutOfRangeException($"{nameof(GetColor)}Index is out of range");
        }

        return colorPallet[index];
    }


    public Sprite GetShape(int index)
    {
        if (index < 0 ||
            index >= shapePallet.Length)
        {
            throw new IndexOutOfRangeException($"{nameof(GetShape)}Index is out of range");
        }

        return shapePallet[index];
    }


    private void DrawBoard(int xSize, int ySize)
    {
        _tiles = new Transform[xSize, ySize];

        for (int y = 0; y < ySize; y++)
        for (int x = 0; x < xSize; x++)
        {
            Transform tile = Instantiate(
                    tilePrefab,
                    new Vector3(x, -y, 0f),
                    Quaternion.identity,
                    tileParent);

            tile.name = $"Tile({x}, {y})";

            _tiles[x, y] = tile;

            DrawDot(x, y);
        }

        OnTilesGenerated?.Invoke();
    }


    private void DrawDot(int x, int y)
    {
        if (x == Width - 1 ||
            y == Height - 1 ||
            Random.value > 0.15f)
        {
            return;
        }

        Vector3 dotPos = new(x + 0.5f, -(y + 0.5f), 0f);

        Transform dot = Instantiate(
                dotPrefab,
                dotPos,
                Quaternion.identity,
                tileParent);

        dot.name = "Dot";

        SpriteRenderer sr = dot.GetComponentInChildren<SpriteRenderer>();

        int index = Random.Range(0, colorPallet.Length);

        sr.color = GetColor(index);
    }
    
    
    public void CheckLine(int boardLine)
    {
        var hits = GetRaycastHits(boardLine);

        var states = AreAllFadedOut(hits);

        if (states == null) return;

        ICommand command = new RemoveSingleLineCommand(states);

        command.Execute();

        OnLineRemoved?.Invoke(boardLine);
    }
    
    private RaycastHit2D[] GetRaycastHits(int boardLine)
    {
        var hits = new RaycastHit2D[Width];

        ContactFilter2D filter = new();

        Vector2 origin = this[0, boardLine].position;

        int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, Width);

        if (result == 0)
        {
            Debug.LogError("CheckLine() caught the empty line!");
        }

        return hits;
    }
    
    private static List<ChipStateManager> AreAllFadedOut([NotNull] RaycastHit2D[] hits)
    {
        if (hits == null) throw new ArgumentNullException(nameof(hits));

        List<ChipStateManager> chips = new();

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.ChipStateManager.CurrentState.GetType() == typeof(FadedInChipState))
            {
                return null;
            }

            chips.Add(chip.GetComponent<ChipStateManager>());
        }

        return chips;
    }

}