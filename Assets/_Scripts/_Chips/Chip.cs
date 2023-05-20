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

    private GameManager _gameManager;
    
    private Tween _tween;

#region INITIALIZATION

    private void Awake()
    {
        _board = Board.Instance;

        _gameManager = GameManager.Instance;
        
        ChipFiniteStateMachine = GetComponent<ChipFiniteStateMachine>();
    }


    public void Init(int shapeIndex, int colorIndex)
    {
        _chipData = new ChipData(shapeIndex, colorIndex);

        SetAppearance();

        ChipFiniteStateMachine.Launch().Forget();
    }


    private void SetAppearance()
    {
        spriteRenderer.sprite = _gameManager.gameData.GetShape(ShapeIndex);

        Color color = _gameManager.gameData.GetColor(ColorIndex);

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
        Vector3 worldPos = _board[boardPos.x, boardPos.y];

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
                        .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].y, FadeTime);
            };

            return;
        }

        _tween = transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].y, MoveTime);
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
                    .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].y, MoveTime)
                    .SetEase(Ease.OutCubic)
                    .ToUniTask();

            return;
        }

        await transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].y, MoveTime)
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

        bool isTopClear = Board.IsPathClear(Vector2.right, _gameManager.gameData.width - chips[0].BoardPosition.x, chips[0], chips[1]);
        bool isBottomClear = Board.IsPathClear(Vector2.left, chips[1].BoardPosition.x, chips[1], chips[0]);

        return isTopClear && isBottomClear;
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