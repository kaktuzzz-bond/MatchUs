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

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public States CurrentState => _chipInfo.state;

    public int ShapeIndex => _chipInfo.shapeIndex;

    public int ColorIndex => _chipInfo.colorIndex;

    public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

    private ChipFiniteStateMachine _chipFiniteStateMachine;

    private ChipInfo _chipInfo;

    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;

        Activate(false);
    }


    public ChipInfo GetInfo()
    {
        _chipInfo.position = transform.position;

        return _chipInfo;
    }


    public void SetState(States newState)
    {
        _chipInfo.state = newState;

        _chipFiniteStateMachine.SetState(newState);
    }


    public void Init(ChipInfo info)
    {
        _chipInfo = info;

        spriteRenderer.sprite = _gameManager.gameData.GetShape(ShapeIndex);

        spriteRenderer.color = _gameManager.gameData.GetColor(ColorIndex, 0f);

        _chipFiniteStateMachine = new ChipFiniteStateMachine(this);
    }

   
    
    [Button("Fade")]
    public async UniTask Fade(float endValue)
    {
        await spriteRenderer
                .DOFade(endValue, _gameManager.gameData.chipFadeTime)
                .SetEase(Ease.Linear)
                .ToUniTask();
    }


    public void Activate(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    public async UniTask MoveToAsync(Vector2Int boardPos)
    {
        Vector3 worldPos = Board.Instance[boardPos.x, boardPos.y];

        await transform
                .DOMove(worldPos, _gameManager.gameData.chipShuffleTime)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
    }


    [Button("Move UP")]
    public async UniTask MoveUpAsync()
    {
        Vector3 startPos = transform.position;

        await transform
                .DOMoveY(++startPos.y, _gameManager.gameData.chipMoveTime)
                .SetEase(Ease.InOutCubic)
                .ToUniTask();
    }


    [Button("Move DOWN")]
    public async UniTask MoveDownAsync()
    {
        Vector3 startPos = transform.position;

        await transform
                .DOMoveY(--startPos.y, _gameManager.gameData.chipMoveTime)
                .SetEase(Ease.InOutCubic)
                .ToUniTask();
    }


    [Button("PLACE ON BOARD")]
    public async UniTask PlaceOnBoardAsync()
    {
        spriteRenderer.transform.localPosition =
                new Vector3(0, _gameManager.gameData.placeOnBoardVerticalShift, 0);

        await spriteRenderer.transform
                .DOLocalMoveY(0, _gameManager.gameData.chipMoveTime)
                .ToUniTask();
    }


    [Button("REMOVE FROM BOARD")]
    public async UniTask RemoveFromBoardAsync()
    {
        await spriteRenderer.transform
                .DOLocalMoveY(
                        _gameManager.gameData.placeOnBoardVerticalShift,
                        _gameManager.gameData.chipMoveTime)
                .ToUniTask();
    }


    public void Destroy()
    {
        Destroy(gameObject);
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

        return LineChecker.IsPathClear(direction, distance, this, other);
    }


    public bool CompareVerticalPosition(Chip other)
    {
        if (BoardPosition.x != other.BoardPosition.x) return false;

        Vector2 direction = other.BoardPosition - BoardPosition;

        float distance = Mathf.Abs(direction.y);

        return LineChecker.IsPathClear(-direction, distance, this, other);
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

        bool isTopClear = LineChecker.IsPathClear(
                Vector2.right,
                _gameManager.gameData.width - chips[0].BoardPosition.x,
                chips[0],
                chips[1]);

        bool isBottomClear = LineChecker.IsPathClear(Vector2.left, chips[1].BoardPosition.x, chips[1], chips[0]);

        return isTopClear && isBottomClear;
    }


//
// #region ENABLE / DISABLE
//
//     private void OnEnable()
//     {
//         _board.OnLineRemoved += MoveUp;
//         _board.OnLineRestored += (x) => MoveDownAsync(x).Forget();
//     }
//
//
//     private void OnDisable()
//     {
//         _board.OnLineRemoved -= MoveUp;
//         _board.OnLineRestored -= MoveDown;
//     }
//
// #endregion
}