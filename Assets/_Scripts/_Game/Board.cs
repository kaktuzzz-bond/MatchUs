using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : Singleton<Board>
{
    public event Action<int> OnLineRemoved;

    public event Action<int> OnLineRestored;

    private HashSet<UniTask> _chipTasksAll = new();

    private GameBoard _gameBoard;

    private LineChecker _lineChecker;

    private GameManager _gameManager;

    public int Capacity => _gameBoard.Capacity;

    private PointerPool _pointerPool;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    public async UniTask Init()
    {
        _gameManager.gameData.tileParent = CreateParent("Tile");
        _gameManager.gameData.chipParent = CreateParent("Chips");
        _gameManager.gameData.pointerParent = CreateParent("Pointers");

        _gameBoard = new(
                _gameManager.gameData.width,
                _gameManager.gameData.height);

        _lineChecker = new LineChecker(_gameBoard);

        _pointerPool = new PointerPool();
        _pointerPool.Init();

        await _gameBoard.DrawBoardAsync();
    }


    private Transform CreateParent(string parentName)
    {
        GameObject go = new();

        Transform parent = go.transform;

        parent.name = parentName;

        parent.SetParent(transform);

        return parent;
    }


    public Vector3 this[int x, int y] => _gameBoard[x, y];


    public void ShowHints(Vector3 firstPosition, Vector3 secondPosition)
    {
        _pointerPool.ShowHints(firstPosition, secondPosition);
    }


    public void HideHints()
    {
        _pointerPool.HideHints();
    }


    public void ShowSelector(Vector3 position)
    {
        _pointerPool.ShowSelector(position);
    }


    public void HideSelector()
    {
        _pointerPool.HideSelector();
    }


    public void CheckLines(Chip first, Chip second)
    {
        int firstLine = first.BoardPosition.y;

        int secondLine = second.BoardPosition.y;

        // a single line
        if (firstLine == secondLine)
        {
            if (_lineChecker.IsLineEmpty(firstLine) != null)
            {
                // ChipController.Instance._commandLogger.
                // AddCommand(new RemoveSingleLineCommand(states));
            }

            return;
        }

        // two lines
        int topLine = Mathf.Min(firstLine, secondLine);

        int bottomLine = Mathf.Max(firstLine, secondLine);

        // CheckLineToRemove(bottomLine);
        //
        // CheckLineToRemove(topLine);
    }


    // private void CheckLineToRemove(int boardLine)
    // {
    //     var hits = GetRaycastHits(boardLine);
    //
    //     var states = AreAllFadedOut(hits);
    //
    //     if (states == null) return;
    //
    //     //ChipController.Instance._commandLogger.AddCommand(new RemoveSingleLineCommand(states));
    //
    //     OnLineRemoved?.Invoke(boardLine);
    // }
    //
    //
    // private RaycastHit2D[] GetRaycastHits(int boardLine)
    // {
    //     var hits = new RaycastHit2D[_gameManager.gameData.width];
    //
    //     ContactFilter2D filter = new();
    //
    //     Vector2 origin = _gameBoard[0, boardLine];
    //
    //     int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, _gameManager.gameData.width);
    //
    //     if (result == 0)
    //     {
    //         Debug.LogError("CheckLine() caught the empty line!");
    //     }
    //
    //     return hits;
    // }
    //
    //
    // private static List<ChipFiniteStateMachine> AreAllFadedOut([NotNull] RaycastHit2D[] hits)
    // {
    //     if (hits == null) throw new ArgumentNullException(nameof(hits));
    //
    //     List<ChipFiniteStateMachine> chips = new();
    //
    //     foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
    //     {
    //         if (!hit.collider.TryGetComponent(out Chip chip)) continue;
    //
    //         if (chip.ChipFiniteStateMachine.CurrentState.GetType() == typeof(LightedOnChipState))
    //         {
    //             return null;
    //         }
    //
    //         chips.Add(chip.GetComponent<ChipFiniteStateMachine>());
    //     }
    //
    //     return chips;
    // }
    //
    //
    // public static bool IsPathClear(Vector2 direction, float distance, [NotNull] Chip origin, [NotNull] Chip other)
    // {
    //     ContactFilter2D filter = new();
    //
    //     List<RaycastHit2D> hits = new();
    //
    //     if (origin.TryGetComponent(out Collider2D component))
    //     {
    //         int count = component.Raycast(direction, filter, hits, distance);
    //     }
    //
    //     foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
    //     {
    //         if (!hit.collider.TryGetComponent(out Chip chip)) continue;
    //
    //         if (chip.Equals(other))
    //         {
    //             continue;
    //         }
    //
    //         if (chip.ChipFiniteStateMachine.CurrentState.GetType() == typeof(LightedOnChipState))
    //         {
    //             return false;
    //         }
    //     }
    //
    //     //Debug.DrawRay(origin.transform.position, direction, Color.red, 5f);
    //
    //     return true;
    // }


    public void RestoreLine(int boardLine)
    {
        //_chipTasksAll.Clear();

        OnLineRestored?.Invoke(boardLine);
    }


    public async UniTask WaitForAllChipTasks()
    {
        //await UniTask.WhenAll(_chipTasksAll);

        //_chipTasksAll.Clear();
    }


    public void AddChipTask(UniTask task)
    {
        //_chipTasksAll.Add(task);
    }


    public static Vector2Int GetBoardPosition(int count)
    {
        int width = GameManager.Instance.gameData.width;

        return new Vector2Int(count % width, count / width);
    }
}