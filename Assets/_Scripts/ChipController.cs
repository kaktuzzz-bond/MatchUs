using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class ChipController : Singleton<ChipController>
{
    public event Action<Chip> OnChipCreated;

    public event Action<int> OnLineRemoved;
    
    [SerializeField]
    private Transform chipPrefab;

    private readonly WaitForSeconds _wait01 = new(0.02f);

#region Component Links

    private Board _board;

    private ChipHandler _handler;

    private GameController _gameController;

    private ChipComparer _chipComparer;

#endregion


    private void Awake()
    {
        _board = Board.Instance;
        _handler = ChipHandler.Instance;
        _gameController = GameController.Instance;
        _chipComparer = ChipComparer.Instance;
    }


    private void DrawStartArray()
    {
        StartCoroutine(DrawStartChipsRoutine());
    }


    private void RemoveLine(int boardLine)
    {
        Debug.Log($"Remove ({boardLine}) line.");
        
        OnLineRemoved?.Invoke(boardLine);
    }


    private void DisableChips(List<ChipStateManager> states)
    {
        foreach (ChipStateManager state in states)
        {
            state.SetDisabledState();
        }
    }


    private void CheckLine(int boardLine)
    {
        
        Debug.Log($"Checking ({boardLine}) line.");
        
        var hits = new RaycastHit2D[_board.Width];

        ContactFilter2D filter = new();

        Vector2 origin = _board[0, boardLine].position;

        int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, _board.Width);

        if (result == 0)
        {
            Debug.LogError("CheckLine() caught the empty line!");

            return;
        }

        var states = AreAllFadedOut(hits);

        if (states == null) return;

        DisableChips(states);
        
        RemoveLine(boardLine);
    }


    private static List<ChipStateManager> AreAllFadedOut([NotNull] RaycastHit2D[] hits)
    {
        if (hits == null) throw new ArgumentNullException(nameof(hits));

        List<ChipStateManager> chips = new();

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.StateManager.CurrentState.GetType() == typeof(FadedInChipState))
            {
                return null;
            }

            chips.Add(chip.GetComponent<ChipStateManager>());
        }

        return chips;
    }


    private void FadeOutChips(Chip first, Chip second)
    {
        first.StateManager.SetFadedOutState();

        second.StateManager.SetFadedOutState();

        int topLine = Mathf.Min(first.BoardPosition.y, second.BoardPosition.y);
        
        int bottomLine = Mathf.Max(first.BoardPosition.y, second.BoardPosition.y);
        
        CheckLine(bottomLine);
        
        CheckLine(topLine);
    }


    private IEnumerator DrawStartChipsRoutine()
    {
        for (int i = 0; i < (int)_gameController.DifficultyLevel; i++)
        {
            CreateChip();

            yield return _wait01;
        }
    }


    private void CreateChip()
    {
        ChipData data = _handler.GetNewChipData();

        Vector2Int boardPos = _handler.NextBoardPosition;

        CreateNewChip(data.shapeIndex, data.colorIndex, boardPos);
    }


    private void CreateNewChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = _board[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, _board.chipParent);

        Chip chip = instance.GetComponent<Chip>();

        chip.Init(shapeIndex, colorIndex, boardPosition);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        OnChipCreated?.Invoke(chip);
    }


#region Enable / Disable

    private void OnEnable()
    {
        _gameController.OnGameStarted += DrawStartArray;
        _chipComparer.OnChipMatched += FadeOutChips;
    }


    private void OnDisable()
    {
        _gameController.OnGameStarted -= DrawStartArray;
        _chipComparer.OnChipMatched -= FadeOutChips;
    }

#endregion
}