using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ChipStateManager))]
public class Chip : MonoBehaviour
{
    public event Action OnInitialized;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    public int ShapeIndex => _chipData.shapeIndex;

    public int ColorIndex => _chipData.colorIndex;

    public const float FadeTime = 0.1f;

    [ShowInInspector]
    public IChipState ChipState => _stateManager.CurrentState;

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector, BoxGroup("Appearance/Chip data"), HideLabel, ReadOnly]
    private ChipData _chipData;

    [VerticalGroup("Position")]
    [ShowInInspector, BoxGroup("Position/Board Position"), HideLabel, ReadOnly]
    public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

    private Board _board;

    private ChipStateManager _stateManager;


    private void Awake()
    {
        _board = Board.Instance;
        _stateManager = GetComponent<ChipStateManager>();
    }


    public void Init(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        _chipData = new ChipData(shapeIndex, colorIndex);

        SetAppearance();

        OnInitialized?.Invoke();
    }


    public void Shake()
    {
        Vector3 shakeStrength = new Vector3(0.2f, 0.2f, 0f);
        transform.DOShakePosition(1f, shakeStrength, 20);
    }


    private void SetAppearance()
    {
        spriteRenderer.sprite = _board.GetShape(ShapeIndex);

        Color color = _board.GetColor(ColorIndex);

        color.a = 0f;

        spriteRenderer.color = color;
    }


    public bool CompareShape(Chip other)
    {
        return ShapeIndex == other.ShapeIndex;
    }


    public bool CompareColor(Chip other)
    {
        return ColorIndex == other.ColorIndex;
    }


    public bool CompareHorizontalPosition(Chip other)
    {
        return BoardPosition.y == other.BoardPosition.y;
    }


    public bool CompareVerticalPosition(Chip other)
    {
        return BoardPosition.x == other.BoardPosition.x;
    }


    public bool CompareMultilinePosition(Chip other)
    {
        int topLine = Mathf.Min(BoardPosition.y, other.BoardPosition.y);
        int bottomLine = Mathf.Max(BoardPosition.y, other.BoardPosition.y);

        return bottomLine - topLine == 1;
    }
}