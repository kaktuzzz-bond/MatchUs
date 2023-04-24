using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ChipStateManager))]
public class Chip : MonoBehaviour
{
    public event Action OnInitialized;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    public const float FadeTime = 0.1f;

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector, BoxGroup("Appearance/Shape"), HideLabel]
    private ChipData _chipData;

    public int ShapeIndex => _chipData.shapeIndex;

    public int ColorIndex => _chipData.colorIndex;

    [VerticalGroup("Position")]
    [ShowInInspector, BoxGroup("Position/Board Position"), HideLabel]
    public Vector2Int BoardPosition { get; private set; }

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

        BoardPosition = boardPosition;

        OnInitialized?.Invoke();
    }


    private void SetAppearance()
    {
        spriteRenderer.sprite = _board.GetShape(ShapeIndex);

        Color color = _board.GetColor(ColorIndex);

        color.a = 0f;
        
        spriteRenderer.color = color;
    }
}