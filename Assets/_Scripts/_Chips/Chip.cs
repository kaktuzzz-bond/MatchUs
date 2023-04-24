using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public event Action OnInitialized;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    public SpriteRenderer Renderer => spriteRenderer;
    
    public const float FadeTime = 0.3f;

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector, BoxGroup("Appearance/Shape"), HideLabel]
    public int ShapeIndex { get; private set; }

    [ShowInInspector, BoxGroup("Appearance/Color"), HideLabel]
    public int ColorIndex { get; private set; }

    [HorizontalGroup("Appearance", 0.5f)]
    [ShowInInspector, BoxGroup("Appearance/Position"), HideLabel]
    public Vector2Int BoardPosition { get; private set; }
    
    private Board _board;
    
    private void Awake()
    {
        _board = Board.Instance;
    }

    [Button("Init Chip")]
    private void Init(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        SetAppearance(shapeIndex, colorIndex);
        
        BoardPosition = boardPosition;
        
        OnInitialized?.Invoke();
    }


    private void SetAppearance(int shapeIndex, int colorIndex)
    {
        ShapeIndex = shapeIndex;

        ColorIndex = colorIndex;

        spriteRenderer.sprite = _board.GetShape(ShapeIndex);

        spriteRenderer.color = _board.GetColor(ColorIndex);
    }
}