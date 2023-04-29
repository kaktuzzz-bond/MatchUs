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
    public event Action<int> OnLineRemoved;

    [SerializeField]
    private Transform chipPrefab;

    private Vector2Int NextBoardPosition =>
            new(_chipCounter % _board.Width, _chipCounter / _board.Width);

    public List<Chip> InGameChips => _inGameChips;

    private int _chipCounter;

    [ShowInInspector]
    private List<Chip> _inGameChips = new();

    [ShowInInspector]
    private List<Chip> _outOfGameChips = new();

    private readonly WaitForSeconds _wait01 = new(0.02f);

#region Component Links

    private Board _board;

    private GameController _gameController;

    private CameraController _cameraController;

    private ChipComparer _chipComparer;

#endregion


    private void Awake()
    {
        _board = Board.Instance;
        _gameController = GameController.Instance;
        _chipComparer = ChipComparer.Instance;
        _cameraController = CameraController.Instance;
    }


    public void Register(Chip chip)
    {
        _inGameChips.Add(chip);

        _outOfGameChips.Remove(chip);

        _chipCounter++;
    }


    public void Unregister(Chip chip)
    {
        _outOfGameChips.Add(chip);

        _inGameChips.Remove(chip);

        _chipCounter--;
    }


    private void DrawStartArray()
    {
        StartCoroutine(DrawStartChipsRoutine(_gameController.ChipsOnStartNumber));
    }


    private void RemoveLine(int boardLine)
    {
        Debug.Log($"Remove ({boardLine}) line.");

        OnLineRemoved?.Invoke(boardLine);
    }


    private void DisableChips(List<ChipStateManager> states)
    {
        foreach (ChipStateManager state in states) state.SetDisabledState();
    }


    private void CheckLine(int boardLine)
    {
        var hits = GetRaycastHits(boardLine);

        var states = AreAllFadedOut(hits);

        if (states == null) return;

        DisableChips(states);

        RemoveLine(boardLine);
    }


    private RaycastHit2D[] GetRaycastHits(int boardLine)
    {
        var hits = new RaycastHit2D[_board.Width];

        ContactFilter2D filter = new();

        Vector2 origin = _board[0, boardLine].position;

        int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, _board.Width);

        if (result == 0)
        {
            Debug.LogError("CheckLine() caught the empty line!");
        }

        return hits;
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

        int firstLine = first.BoardPosition.y;

        int secondLine = second.BoardPosition.y;

        if (firstLine == secondLine)
        {
            CheckLine(firstLine);

            return;
        }

        int topLine = Mathf.Min(first.BoardPosition.y, second.BoardPosition.y);

        int bottomLine = Mathf.Max(first.BoardPosition.y, second.BoardPosition.y);

        CheckLine(bottomLine);

        CheckLine(topLine);
    }


    private IEnumerator DrawStartChipsRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateChip();

            yield return _wait01;
        }
    }


    public void CreateChip(int shapeIndex, int colorIndex)
    {
        Vector2Int boardPos = NextBoardPosition;

        CreateNewChip(shapeIndex, colorIndex, boardPos);
    }


    private void CreateChip()
    {
        ChipData data = GetChipDataByChance();

        Vector2Int boardPos = NextBoardPosition;

        CreateNewChip(data.shapeIndex, data.colorIndex, boardPos);
    }


    private void CreateNewChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = _board[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, _board.chipParent);

        Chip chip = instance.GetComponent<Chip>();

        chip.Init(shapeIndex, colorIndex, boardPosition);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";
    }


    public ChipData GetChipDataByChance()
    {
        return Random.value <= _gameController.ChanceForRandom
                ? GetRandomChipData()
                : GetChipDataForHardLevel();
    }


    private ChipData GetRandomChipData()
    {
        int shapeIndex = Random.Range(0, _board.ShapePalletLength);

        int colorIndex = Random.Range(0, _board.ColorPalletLength);

        return new ChipData(shapeIndex, colorIndex);
    }


    private ChipData GetChipDataForHardLevel()
    {
        var shapeIndexes = Utils.GetIndexes(_board.ShapePalletLength);
        var colorIndexes = Utils.GetIndexes(_board.ColorPalletLength);

        if (_chipCounter > 0)
        {
            shapeIndexes.Remove(_inGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_inGameChips.Last().ColorIndex);

            if (_chipCounter >= _board.Width)
            {
                Chip chip = _inGameChips[^_board.Width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }


#region Enable / Disable

    private void OnEnable()
    {
        _cameraController.OnCameraSetup += DrawStartArray;
        _chipComparer.OnChipMatched += FadeOutChips;
    }


    private void OnDisable()
    {
        _cameraController.OnCameraSetup -= DrawStartArray;
        _chipComparer.OnChipMatched -= FadeOutChips;
    }

#endregion
}