using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Chip : MonoBehaviour
{
    public enum States
    {
        LightOn,

        LightOff,

        Removed
    }

    public States CurrentState { get; private set; }

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int ShapeIndex { get; private set; }

    public int ColorIndex { get; private set; }

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


    private void Awake()
    {
        _board = Board.Instance;

        _gameManager = GameManager.Instance;
    }


    public void Init(ChipInfo info)
    {
        Activate(false);

        ShapeIndex = info.shapeIndex;

        ColorIndex = info.colorIndex;

        CurrentState = info.state;

        spriteRenderer.sprite = _gameManager.gameData.GetShape(ShapeIndex);

        spriteRenderer.color = _gameManager.gameData.GetColor(ColorIndex, 0f);

        ChipFiniteStateMachine = new ChipFiniteStateMachine(this);
    }


    [Button("Fade")]
    public async UniTask Fade(float endValue)
    {
        await spriteRenderer
                .DOFade(endValue, _gameManager.gameData.chipFadeTime)
                .SetEase(Ease.Linear)
                .ToUniTask();
    }


    public void Activate(bool isActive) => gameObject.SetActive(isActive);


    public async UniTask VerticalShiftAsync(float targetY)
    {
        await transform
                .DOMoveY(targetY, _gameManager.gameData.chipFadeTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    public async UniTask MoveToAsync(Vector2Int boardPos)
    {
        Vector3 worldPos = _board[boardPos.x, boardPos.y];

        await transform
                .DOMove(worldPos, _gameManager.gameData.chipShuffleTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }


    private void MoveUp(int boardLine)
    {
        if (BoardPosition.y <= boardLine) return;

        if (_tween.IsActive())
        {
            _tween.onComplete += () =>
            {
                _tween = transform
                        .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].y, _gameManager.gameData.chipFadeTime);
            };

            return;
        }

        _tween = transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y - 1].y, _gameManager.gameData.chipMoveTime);
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
                    .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].y, _gameManager.gameData.chipMoveTime)
                    .SetEase(Ease.OutCubic)
                    .ToUniTask();

            return;
        }

        await transform
                .DOMoveY(_board[BoardPosition.x, BoardPosition.y + 1].y, _gameManager.gameData.chipMoveTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    [Button("PLACE ON BOARD")]
    private async UniTask PlaceOnBoard()
    {
        spriteRenderer.transform.localPosition =
                new Vector3(0, _gameManager.gameData.placeOnBoardVerticalShift, 0);

        await spriteRenderer.transform
                .DOLocalMoveY(0, _gameManager.gameData.chipMoveTime)
                .ToUniTask();
    }


    [Button("REMOVE FROM BOARD")]
    private async UniTask RemoveFromBoard()
    {
        await spriteRenderer.transform
                .DOLocalMoveY(
                        _gameManager.gameData.placeOnBoardVerticalShift,
                        _gameManager.gameData.chipMoveTime)
                .ToUniTask();
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

        bool isTopClear = Board.IsPathClear(
                Vector2.right,
                _gameManager.gameData.width - chips[0].BoardPosition.x,
                chips[0],
                chips[1]);

        bool isBottomClear = Board.IsPathClear(Vector2.left, chips[1].BoardPosition.x, chips[1], chips[0]);

        return isTopClear && isBottomClear;
    }


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