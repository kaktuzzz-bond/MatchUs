using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ChipFiniteStateMachine))]
public class Chip : MonoBehaviour
{
    public event Action OnInitialized;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int ShapeIndex => _chipData.shapeIndex;

    public int ColorIndex => _chipData.colorIndex;

    private const float FadeTime = 0.2f;

    private const float MoveTime = 0.5f;

    public ChipFiniteStateMachine ChipFiniteStateMachine { get; private set; }

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector] [BoxGroup("Appearance/Chip data")] [HideLabel] [ReadOnly]
    private ChipData _chipData;

    [VerticalGroup("Position")]
    [ShowInInspector] [BoxGroup("Position/Board Position")] [HideLabel] [ReadOnly]
    public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

    private Board _board;

    private ChipController _chipController;

    private Tween _tween;

#region INITIALIZATION

    private void Awake()
    {
        _board = Board.Instance;
        _chipController = ChipController.Instance;

        ChipFiniteStateMachine = GetComponent<ChipFiniteStateMachine>();
    }


    public void Init(int shapeIndex, int colorIndex)
    {
        _chipData = new ChipData(shapeIndex, colorIndex);

        SetAppearance();

        OnInitialized?.Invoke();
    }


    private void SetAppearance()
    {
        spriteRenderer.sprite = _board.GetShape(ShapeIndex);

        Color color = _board.GetColor(ColorIndex);

        color.a = 0f;

        spriteRenderer.color = color;
    }

#endregion

#region COMMANDS

    [Button("Fade")]
    public void Fade(float endValue)
    {
        spriteRenderer
                .DOFade(endValue, FadeTime)
                .SetEase(Ease.Linear);
    }


    [Button("Activate")]
    public void Activate(bool isActive) => gameObject.SetActive(isActive);


    public void VerticalShiftTo(float targetY)
    {
        transform
                .DOMoveY(targetY, FadeTime)
                .SetEase(Ease.Linear);
    }


    public void VerticalShiftTo(float targetY, TweenCallback doOnComplete)
    {
        transform
                .DOMoveY(targetY, FadeTime)
                .SetEase(Ease.Linear)
                .OnComplete(doOnComplete);
    }


    public void MoveTo(Vector2Int boardPos)
    {
        Vector3 worldPos = _board[boardPos.x, boardPos.y].position;

        transform
                .DOMove(worldPos, MoveTime)
                .SetEase(Ease.OutCubic);
    }

#endregion

    // public void Shake()
    // {
    //     Vector3 shakeStrength = new Vector3(0.2f, 0.2f, 0f);
    //     transform.DOShakePosition(1f, shakeStrength, 20);
    // }

#region MOVING

    private void MoveUp(int boardLine)
    {
        if (BoardPosition.y <= boardLine) return;

        if (_tween.IsActive())
        {
            _tween.onComplete += () =>
            {
                _tween = transform
                        .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].position.y, FadeTime);
            };

            return;
        }

        _tween = transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].position.y, MoveTime);
    }


    private void MoveDown(int boardLine)
    {
        if (BoardPosition.y < boardLine ||
            ChipFiniteStateMachine.CurrentState.GetType() == typeof(DisabledChipState)) return;

        Debug.Log($"Move down in ({boardLine})");

        if (_tween.IsActive())
        {
            _tween.onComplete += () =>
            {
                _tween = transform
                        .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].position.y, FadeTime);
            };

            return;
        }

        _tween = transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].position.y, MoveTime);
    }

#endregion

#region COMPARISON

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

        return Board.IsPathClear(direction, distance, this, other);
    }


    public bool CompareVerticalPosition(Chip other)
    {
        if (BoardPosition.x != other.BoardPosition.x) return false;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.y);

        return Board.IsPathClear(-direction, distance, this, other);
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

        bool isTopClear = Board.IsPathClear(Vector2.right, _board.Width - chips[0].BoardPosition.x, chips[0], chips[1]);
        bool isBottomClear = Board.IsPathClear(Vector2.left, chips[1].BoardPosition.x, chips[1], chips[0]);

        return isTopClear && isBottomClear;
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        _board.OnLineRemoved += MoveUp;
        _chipController.OnLineRestored += MoveDown;
    }


    private void OnDisable()
    {
        _board.OnLineRemoved -= MoveUp;
        _chipController.OnLineRestored -= MoveDown;
    }

#endregion
}