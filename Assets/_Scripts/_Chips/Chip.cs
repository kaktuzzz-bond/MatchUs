using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ChipFiniteStateMachine))]
public class Chip : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int ShapeIndex => _chipData.shapeIndex;

    public int ColorIndex => _chipData.colorIndex;

    private const float FadeTime = 0.2f;

    private const float MoveTime = 0.3f;

    private const float ShuffleTime = 0.6f;

    public ChipFiniteStateMachine ChipFiniteStateMachine { get; private set; }

    [HorizontalGroup("Appearance", Title = "Chip Settings")]
    [ShowInInspector] [BoxGroup("Appearance/Chip data")] [HideLabel] [ReadOnly]
    private ChipData _chipData;

    [VerticalGroup("Position")]
    [ShowInInspector] [BoxGroup("Position/Board Position")] [HideLabel] [ReadOnly]
    public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

    private Board _board;

    private LineDrawer _lineDrawer;

    private Tween _tween;

#region INITIALIZATION

    private void Awake()
    {
        _board = Board.Instance;

        ChipFiniteStateMachine = GetComponent<ChipFiniteStateMachine>();

        _lineDrawer = LineDrawer.Instance;
    }


    public void Init(int shapeIndex, int colorIndex)
    {
        _chipData = new ChipData(shapeIndex, colorIndex);

        SetAppearance();

        ChipFiniteStateMachine.Launch().Forget();
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


    public void Activate(bool isActive) => gameObject.SetActive(isActive);


    public async UniTask VerticalShiftAsync(float targetY)
    {
        await transform
                .DOMoveY(targetY, FadeTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    public async UniTask MoveToAsync(Vector2Int boardPos)
    {
        Vector3 worldPos = _board[boardPos.x, boardPos.y].position;

        await transform
                .DOMove(worldPos, ShuffleTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }

#endregion

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
        if (BoardPosition.y < boardLine) return;
        
        _board.AddChipTask(MoveDownAsync());
    }

    private async UniTask MoveDownAsync()
    {
        if (_tween.IsActive())
        {
            await _tween
                    .ToUniTask();

            await transform
                    .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].position.y, MoveTime)
                    .SetEase(Ease.OutCubic)
                    .ToUniTask();

            return;
        }

        await transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].position.y, MoveTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
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


    public Vector3[] CompareHorizontalPosition(Chip other)
    {
        if (BoardPosition.y != other.BoardPosition.y) return null;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.x);

        if (!Board.IsPathClear(direction, distance, this, other)) return null;

        return _lineDrawer.GetLinePoints(
                other.BoardPosition,
                (int)distance,
                direction.x < 0 ? LineDrawer.LineDirection.Right : LineDrawer.LineDirection.Left);
    }


    public Vector3[] CompareVerticalPosition(Chip other)
    {
        if (BoardPosition.x != other.BoardPosition.x) return null;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.y);

        if (!Board.IsPathClear(-direction, distance, this, other)) return null;

        return _lineDrawer.GetLinePoints(
                other.BoardPosition,
                (int)distance,
                direction.y > 0 ? LineDrawer.LineDirection.Up : LineDrawer.LineDirection.Down);
    }


    public List<Vector3[]> CompareMultilinePosition(Chip other)
    {
        if (Mathf.Abs(BoardPosition.y - other.BoardPosition.y) != 1) return null;

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

        float topDistance = _board.Width - chips[0].BoardPosition.x;

        bool isTopClear = Board.IsPathClear(Vector2.right, topDistance, chips[0], chips[1]);

        float bottomDistance = chips[1].BoardPosition.x;

        bool isBottomClear = Board.IsPathClear(Vector2.left, bottomDistance, chips[1], chips[0]);

        if (!(isTopClear && isBottomClear)) return null;

        var topLine = _lineDrawer.GetLinePoints(
                chips[0].BoardPosition,
                (int)topDistance - 1,
                LineDrawer.LineDirection.Right);

        var bottomLine = _lineDrawer.GetLinePoints(
                chips[1].BoardPosition,
                (int)bottomDistance,
                LineDrawer.LineDirection.Left);

        return new List<Vector3[]> { topLine, bottomLine };
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        _board.OnLineRemoved += MoveUp;
        _board.OnLineRestored += MoveDown;
    }


    private void OnDisable()
    {
        _board.OnLineRemoved -= MoveUp;
        _board.OnLineRestored -= MoveDown;
    }

#endregion
}