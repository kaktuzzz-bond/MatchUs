using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
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

    public ChipStateManager StateManager { get; private set; }

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector, BoxGroup("Appearance/Chip data"), HideLabel, ReadOnly]
    private ChipData _chipData;

    [VerticalGroup("Position")]
    [ShowInInspector, BoxGroup("Position/Board Position"), HideLabel, ReadOnly]
    public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

    private Board _board;

    private Collider2D _collider;


    private void Awake()
    {
        _board = Board.Instance;
        StateManager = GetComponent<ChipStateManager>();
        _collider = GetComponent<Collider2D>();
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
        if (BoardPosition.y != other.BoardPosition.y) return false;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.x);

        return IsPathClear(direction, distance, other);
    }


    public bool CompareVerticalPosition(Chip other)
    {
        if (BoardPosition.x != other.BoardPosition.x) return false;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.y);

        return IsPathClear(-direction, distance, other);
    }


    public bool CompareMultilinePosition(Chip other)
    {
        if (Mathf.Abs(BoardPosition.y - other.BoardPosition.y) != 1) return false;

        var chips = new Chip[2];

        if (BoardPosition.y > other.BoardPosition.y)
        {
            chips[0] = other;
            chips[1] = this;
        }
        else
        {
            chips[0] = this;
            chips[1] = other;
        }

        bool isTopClear = chips[0].IsPathClear(Vector2.right, _board.Width - chips[0].BoardPosition.x, chips[1]);
        bool isBottomClear = chips[1].IsPathClear(Vector2.left,  chips[1].BoardPosition.x, chips[0]);
        
        return isTopClear && isBottomClear;
    }


    public bool IsPathClear(Vector2 direction, float distance, Chip other)
    {
        ContactFilter2D filter = new();

        List<RaycastHit2D> hits = new();

        int count = _collider.Raycast(direction, filter, hits, distance);

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.Equals(other)) continue;
            
            if (chip.StateManager.CurrentState.GetType() == typeof(FadedInChipState))
            {
                return false;
            }
        }

        Debug.DrawRay(transform.position, direction, Color.red, 5f);
        
        return true;
    }
}