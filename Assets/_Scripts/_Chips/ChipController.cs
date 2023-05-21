using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
    [SerializeField]
    private float delayOnDrawChipsInSeconds = 0.08f;

    public Vector2Int NextBoardPosition =>
            new(
                    ChipRegistry.Counter % _gameManager.gameData.width,
                    ChipRegistry.Counter / _gameManager.gameData.width);

    public ChipRegistry ChipRegistry { get; private set; }

    public CommandLogger Log { get; private set; }

    public PointerController PointerController { get; private set; }

    private ChipComparer _chipComparer;

    private ChipInfoGenerator _chipInfoGenerator;

    private GameManager _gameManager;


    private void Awake()
    {
        ChipRegistry = new ChipRegistry();

        Log = new CommandLogger();

        //PointerController = new PointerController(selectorPrefab, hintPrefab);

        _chipInfoGenerator = new ChipInfoGenerator();

        _chipComparer = new ChipComparer(PointerController);

        _gameManager = GameManager.Instance;
    }


    public void AddChips()
    {
        PointerController.HidePointers();

        _chipComparer.ClearStorage();

        Log.AddCommand(new AddChipsCommand());
    }


    public void ShuffleChips()
    {
        PointerController.HidePointers();

        _chipComparer.ClearStorage();

        Log.AddCommand(new ShuffleCommand());
    }


    public void ShowHints()
    {
        PointerController.ShowHints();
    }


    public void UndoCommand()
    {
        Log.UndoCommand().Forget();
    }


    public void ProcessTappedChip(Chip chip)
    {
        _chipComparer.TryMatching(chip);
    }


    public async UniTask DrawStartArrayAsync()
    {
        var chips = _chipInfoGenerator.GetStartChipInfoArray();

        foreach (ChipInfo info in chips) { }

        GameManager.Instance.StartGame();

        Log.CheckStackCount();
    }


    private async UniTaskVoid DrawStartArrayAsync(int count)
    {
        int line = NextBoardPosition.y;

        for (int i = 0; i < count; i++)
        {
            //ChipData data = _randomizer.GetChipDataByChance();

            Vector2Int boardPos = NextBoardPosition;

            if (NextLine(ref line, boardPos.y))
            {
                await UniTask.Delay(1000);
            }

            // DrawChip(data.shapeIndex, data.colorIndex, boardPos);
        }

        await UniTask.Yield();
    }


    public async UniTask<List<Chip>> CloneInGameChipsAsync()
    {
        List<Chip> added = new();

        var chips = ChipRegistry.ActiveChips;

        int line = NextBoardPosition.y;

        foreach (Chip chip in chips)
        {
            Vector2Int boardPos = NextBoardPosition;

            if (NextLine(ref line, boardPos.y))
            {
                await UniTask.Delay(1000);

                CameraController.Instance.MoveToBottomBound();
            }

            //Chip newChip = DrawChip(chip.ShapeIndex, chip.ColorIndex, boardPos);

            //added.Add(newChip);
        }

        return added;
    }


    private bool NextLine(ref int line, int boardPosY)
    {
        if (boardPosY == line) return false;

        line = boardPosY;

        return true;
    }


    public async UniTask RemoveChipsAsync(List<Chip> chips)
    {
        int line = chips[0].BoardPosition.y;

        foreach (Chip chip in chips)
        {
            if (NextLine(ref line, chip.BoardPosition.y))
            {
                await UniTask.Delay(1000);

                CameraController.Instance.MoveToBottomBound();
            }

            //chip.ChipFiniteStateMachine.SetSelfDestroyableState().Forget();

            ChipRegistry.CheckBoardCapacity();
        }
    }


    private Chip DrawChip(ChipInfo info)
    {
        Transform instance = Instantiate(
                _gameManager.gameData.chipPrefab,
                info.position,
                Quaternion.identity,
                _gameManager.gameData.chipParent);

        if (!instance.TryGetComponent(out Chip chip)) return null;

        instance.name = $"Chip ({info.shapeIndex}, {info.colorIndex})";

        chip.Init(info);

        return chip;
    }


#region ENABLE / DISABLE

    // private void OnEnable()
    // {
    //     CameraController.Instance.OnCameraSetup += DrawStartArray;
    // }
    //
    //
    // private void OnDisable()
    // {
    //     CameraController.Instance.OnCameraSetup -= DrawStartArray;
    // }

#endregion
}