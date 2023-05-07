using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Sirenix.OdinInspector;

[RequireComponent(typeof(ChipRegistry))]
public class ChipController : Singleton<ChipController>
{
    public event Action<int> OnLineRestored;

    [SerializeField]
    private Transform chipPrefab;

    private Vector2Int NextBoardPosition =>
            new(_chipRegistry.Counter % _board.Width, _chipRegistry.Counter / _board.Width);

    [ShowInInspector]
    public Stack<ICommand> LogStack => Log.Stack;

    public CommandLogger Log { get; } = new();

    private List<Chip> _addedChips;

#region COMPONENTS LINKS

    private Board _board;

    private GameManager _gameManager;

    private ChipRegistry _chipRegistry;

#endregion


    private void Awake()
    {
        _board = Board.Instance;
        _gameManager = GameManager.Instance;
        _chipRegistry = ChipRegistry.Instance;
    }


  
    
    public static void AddChips()
    {
        PointerController.Instance.HidePointers();

        ChipComparer.Instance.ClearStorage();
        
        ICommand command = new AddChipsCommand();

        command.Execute();
    }


    [Button("Restore Line From")]
    public void RestoreLine(int boardLine)
    {
        OnLineRestored?.Invoke(boardLine);
    }


#region PLACE ON BOARD

    private void DrawStartArray()
    {
        StartCoroutine(DrawStartChipsRoutine(_gameManager.ChipsOnStartNumber));
    }


    private IEnumerator DrawStartChipsRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _ = CreateChip();

            yield return null;
        }
    }


    public void CloneInGameChips(List<Chip> chips, out List<Chip> addedChips)
    {
        _addedChips = new List<Chip>();

        StartCoroutine(CloneInGameChipsRoutine(chips));

        addedChips = _addedChips;
    }


    private IEnumerator CloneInGameChipsRoutine(List<Chip> chips)
    {
        foreach (Chip newChip in chips.Select(chip => CreateChip(chip.ShapeIndex, chip.ColorIndex)))
        {
            yield return null;

            _addedChips.Add(newChip);
        }
    }

#endregion

#region CHIP CREATION

    private Chip CreateChip(int shapeIndex, int colorIndex)
    {
        Vector2Int boardPos = NextBoardPosition;

        return CreateNewChip(shapeIndex, colorIndex, boardPos);
    }


    private Chip CreateChip()
    {
        ChipData data = GetChipDataByChance();

        Vector2Int boardPos = NextBoardPosition;

        return CreateNewChip(data.shapeIndex, data.colorIndex, boardPos);
    }


    private Chip CreateNewChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = _board[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, _board.chipParent);

        if (!instance.TryGetComponent(out Chip chip)) return null;

        chip = instance.GetComponent<Chip>();

        chip.Init(shapeIndex, colorIndex);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        return chip;
    }

#endregion

#region CHIP DATA

    private ChipData GetChipDataByChance()
    {
        return Random.value <= _gameManager.ChanceForRandom
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
        List<int> shapeIndexes = new(_board.ShapeIndexes);
        List<int> colorIndexes = new(_board.ColorIndexes);

        if (_chipRegistry.Counter > 0)
        {
            shapeIndexes.Remove(_chipRegistry.InGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_chipRegistry.InGameChips.Last().ColorIndex);

            if (_chipRegistry.Counter >= _board.Width)
            {
                Chip chip = _chipRegistry.InGameChips[^_board.Width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        GameSceneGUI.Instance.OnFaderDisappeared += DrawStartArray;
    }


    private void OnDisable()
    {
        GameSceneGUI.Instance.OnFaderDisappeared -= DrawStartArray;
    }

#endregion
}