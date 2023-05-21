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

    private GameManager _gameManager;

    public int Capacity => _gameBoard.Capacity;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    public void Init(GameBoard gameBoard)
    {
        _gameBoard = gameBoard;

        _gameManager.gameData.tileParent = CreateParent("Tile");
        _gameManager.gameData.chipParent = CreateParent("Chips");
        _gameManager.gameData.pointerParent = CreateParent("Pointers");
    }


    private Transform CreateParent(string parentName)
    {
        GameObject go = new();

        Transform parent = go.transform;

        parent.name = parentName;

        return parent;
    }


    public Vector3 this[int x, int y] => _gameBoard[x, y];


    public async UniTaskVoid ProcessMatched(Chip first, Chip second)
    {
        // fade out chips
        ChipController.Instance.Log.AddCommand(new FadeOutCommand(first, second));

        // lines cash
        int firstLine = first.BoardPosition.y;

        int secondLine = second.BoardPosition.y;

        // a single line
        if (firstLine == secondLine)
        {
            CheckLineToRemove(firstLine);

            return;
        }

        // two lines
        int topLine = Mathf.Min(firstLine, secondLine);

        int bottomLine = Mathf.Max(firstLine, secondLine);

        CheckLineToRemove(bottomLine);

        CheckLineToRemove(topLine);

        await UniTask.Yield();
    }


    private void CheckLineToRemove(int boardLine)
    {
        var hits = GetRaycastHits(boardLine);

        var states = AreAllFadedOut(hits);

        if (states == null) return;

        ChipController.Instance.Log.AddCommand(new RemoveSingleLineCommand(states));

        OnLineRemoved?.Invoke(boardLine);
    }


    private RaycastHit2D[] GetRaycastHits(int boardLine)
    {
        var hits = new RaycastHit2D[_gameManager.gameData.width];

        ContactFilter2D filter = new();

        Vector2 origin = _gameBoard[0, boardLine];

        int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, _gameManager.gameData.width);

        if (result == 0)
        {
            Debug.LogError("CheckLine() caught the empty line!");
        }

        return hits;
    }


    private static List<ChipFiniteStateMachine> AreAllFadedOut([NotNull] RaycastHit2D[] hits)
    {
        if (hits == null) throw new ArgumentNullException(nameof(hits));

        List<ChipFiniteStateMachine> chips = new();

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.ChipFiniteStateMachine.CurrentState.GetType() == typeof(LightedOnChipState))
            {
                return null;
            }

            chips.Add(chip.GetComponent<ChipFiniteStateMachine>());
        }

        return chips;
    }


    public static bool IsPathClear(Vector2 direction, float distance, [NotNull] Chip origin, [NotNull] Chip other)
    {
        ContactFilter2D filter = new();

        List<RaycastHit2D> hits = new();

        if (origin.TryGetComponent(out Collider2D component))
        {
            int count = component.Raycast(direction, filter, hits, distance);
        }

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.Equals(other))
            {
                continue;
            }

            if (chip.ChipFiniteStateMachine.CurrentState.GetType() == typeof(LightedOnChipState))
            {
                return false;
            }
        }

        Debug.DrawRay(origin.transform.position, direction, Color.red, 5f);

        return true;
    }


    public void RestoreLine(int boardLine)
    {
        _chipTasksAll.Clear();

        OnLineRestored?.Invoke(boardLine);
    }


    public async UniTask WaitForAllChipTasks()
    {
        await UniTask.WhenAll(_chipTasksAll);

        _chipTasksAll.Clear();
    }


    public void AddChipTask(UniTask task)
    {
        _chipTasksAll.Add(task);
    }
}