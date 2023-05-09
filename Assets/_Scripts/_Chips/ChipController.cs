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

    public event Action<List<Chip>> OnChipsAdded;

    [SerializeField]
    private Transform chipPrefab;

    public Vector2Int NextBoardPosition =>
            new(_chipRegistry.Counter % _board.Width, _chipRegistry.Counter / _board.Width);

    [ShowInInspector]
    public Stack<ICommand> LogStack => Log.Stack;

    public CommandLogger Log { get; } = new();

    private ChipComparer _comparer;

    public List<Chip> AddedChips { get; private set; } = new();

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
        _comparer = new ChipComparer(PointerController.Instance);
    }


    public void AddChips()
    {
        //if (!GameManager.Instance.AllowInput) return;

        PointerController.Instance.HidePointers();

        _comparer.ClearStorage();

        ICommand command = new AddChipsCommand();

        command.Execute();
    }


    public void ShuffleChips()
    {
        //if (!GameManager.Instance.AllowInput) return;

        PointerController.Instance.HidePointers();

        _comparer.ClearStorage();

        ICommand command = new ShuffleCommand();

        command.Execute();
    }


    public void RestoreLine(int boardLine)
    {
        OnLineRestored?.Invoke(boardLine);
    }


    public void ProcessTappedChip(Chip chip)
    {
        var matched = _comparer.IsMatching(chip);

        if (matched != null)
        {
            Board.Instance.ProcessMatched(matched[0], matched[1]);
        }
    }


#region PLACE ON BOARD

    public void DrawStartArray()
    {
        StartCoroutine(DrawStartArrayRoutine(_gameManager.ChipsOnStartNumber));
    }


    private IEnumerator DrawStartArrayRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateChip();

            yield return null;
        }

        GameManager.Instance.StartGame();
    }


    public void CloneInGameChips()
    {
        StartCoroutine(CloneInGameChipsRoutine());
    }


    private IEnumerator CloneInGameChipsRoutine()
    {
        AddedChips.Clear();

        var chips = ChipRegistry.Instance.ActiveChips;

        foreach (Chip newChip in chips.Select(chip => CreateChip(chip.ShapeIndex, chip.ColorIndex)))
        {
            AddedChips.Add(newChip);

            yield return null;
        }

        OnChipsAdded?.Invoke(AddedChips);
    }


    public void RemoveChips(List<Chip> chips)
    {
        StartCoroutine(RemoveChipsRoutine(chips));
    }

    private IEnumerator RemoveChipsRoutine(List<Chip> chips)
    {
        foreach (Chip chip in chips)
        {
            chip.ChipFiniteStateMachine.SetSelfDestroyableState();
            
            yield return null;
        }

        ChipRegistry.Instance.CheckBoardCapacity();
    }
    
#endregion

#region CHIP CREATION

    private Chip CreateChip(int shapeIndex, int colorIndex)
    {
        return CreateNewChip(shapeIndex, colorIndex, NextBoardPosition);
    }


    private Chip CreateChip()
    {
        ChipData data = GetChipDataByChance();

        return CreateNewChip(data.shapeIndex, data.colorIndex, NextBoardPosition);
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
}