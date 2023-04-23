using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChipStateManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [ShowInInspector]
    private IChip _currentState = new ChipRestoredState();

    private readonly ChipRestoredState restoredState = new();

    private readonly ChipFadedOutState fadedOutState = new();

    private readonly ChipFadedInState fadedInState = new();
    
    private readonly ChipDisabledState disabledState = new();
    
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

    [Button("Init")]
    private void Init(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        gameObject.SetActive(false);

        SetAppearance(shapeIndex, colorIndex);

        _currentState = restoredState;

        _currentState.Enter(this);
    }


    [Button("Set Restored State")]
    public void SetRestoredState() => SetState(restoredState);

    [Button("Set Faded Out State")]
    public void SetFadedOutState() => SetState(fadedOutState);

    [Button("Set Faded In State")]
    public void SetFadedInState() => SetState(fadedInState);
    
    [Button("Set Disabled State")]
    public void SetDisabledState() => SetState(disabledState);


    private void SetState(IChip newState)
    {
        _currentState = newState;
        _currentState.Enter(this);
    }


    private void SetAppearance(int shapeIndex, int colorIndex)
    {
        ShapeIndex = shapeIndex;

        ColorIndex = colorIndex;

        spriteRenderer.sprite = _board.GetShape(ShapeIndex);

        spriteRenderer.color = _board.GetColor(ColorIndex);
    }
}