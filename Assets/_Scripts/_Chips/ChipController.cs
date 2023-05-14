using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class ChipController : Singleton<ChipController>
{
  

    [SerializeField]
    private Transform chipPrefab;

    private const int DelayOnDrawChips = 50;

    public Vector2Int NextBoardPosition =>
            new(ChipRegistry.Counter % Board.Instance.Width, ChipRegistry.Counter / Board.Instance.Width);

    public ChipRegistry ChipRegistry { get; private set; }

    public CommandLogger Log { get; private set; }

    private ChipComparer _comparer;

    private ChipRandomizer _randomizer;


    private void Awake()
    {
        ChipRegistry = new ChipRegistry();

        Log = new CommandLogger();

        _randomizer = new ChipRandomizer(ChipRegistry, Board.Instance);

        _comparer = new ChipComparer(PointerController.Instance);
    }


    public void AddChips()
    {
        PointerController.Instance.HidePointers();

        _comparer.ClearStorage();

        Log.ExecuteAndAdd(new AddChipsCommand());
    }


    public void ShuffleChips()
    {
        PointerController.Instance.HidePointers();

        _comparer.ClearStorage();

        Log.ExecuteAndAdd(new ShuffleCommand());
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

    private void DrawStartArray()
    {
        DrawStartArrayAsync(GameManager.Instance.ChipsOnStartNumber).Forget();

        GameManager.Instance.StartGame();
    }


    private async UniTaskVoid DrawStartArrayAsync(int count)
    {
        int line = NextBoardPosition.y;

        for (int i = 0; i < count; i++)
        {
            ChipData data = _randomizer.GetChipDataByChance();

            Vector2Int boardPos = NextBoardPosition;

            if (boardPos.y != line)
            {
                line = boardPos.y;
                
                await UniTask.Delay(DelayOnDrawChips);
            }

            CreateChip(data.shapeIndex, data.colorIndex, boardPos);
        }

        await UniTask.Yield();
    }


    public async UniTask<List<Chip>> CloneInGameChipsAsync()
    {
        List<Chip> added = new();

        var chips = ChipRegistry.ActiveChips;

        foreach (Chip newChip in chips.Select(chip => CreateChip(chip.ShapeIndex, chip.ColorIndex, NextBoardPosition)))
        {
            added.Add(newChip);

            await UniTask.Yield();
        }

        return added;
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

        ChipRegistry.CheckBoardCapacity();
    }

#endregion


    private Chip CreateChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = Board.Instance[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, Board.Instance.chipParent);

        if (!instance.TryGetComponent(out Chip chip)) return null;

        chip.Init(shapeIndex, colorIndex);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        return chip;
    }


#region ENABLE / DISABLE

    private void OnEnable()
    {
        CameraController.Instance.OnCameraSetup += DrawStartArray;
    }


    private void OnDisable()
    {
        CameraController.Instance.OnCameraSetup -= DrawStartArray;
    }

#endregion
}