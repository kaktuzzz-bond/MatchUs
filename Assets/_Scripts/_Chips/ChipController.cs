using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
    [SerializeField] [FoldoutGroup("Prefabs")]
    private Transform chipPrefab;

    [SerializeField] [FoldoutGroup("Prefabs")]
    private GamePointer selectorPrefab;

    [SerializeField] [FoldoutGroup("Prefabs")]
    private GamePointer hintPrefab;

    [SerializeField]
    private float delayOnDrawChipsInSeconds = 0.08f;

    public Vector2Int NextBoardPosition =>
            new(ChipRegistry.Counter % _gameManager.gameData.width, 
                    ChipRegistry.Counter / _gameManager.gameData.width);

    public ChipRegistry ChipRegistry { get; private set; }

    public CommandLogger Log { get; private set; }

    public PointerController PointerController { get; private set; }

    private ChipComparer _comparer;

    private ChipRandomizer _randomizer;

    private int DelayOnDrawChips => (int)(delayOnDrawChipsInSeconds * 1000);


    private GameManager _gameManager;
    
    private void Awake()
    {
        ChipRegistry = new ChipRegistry();

        Log = new CommandLogger();

        PointerController = new PointerController(selectorPrefab, hintPrefab);

        _randomizer = new ChipRandomizer(ChipRegistry, Board.Instance, GameManager.Instance);

        _comparer = new ChipComparer(PointerController);

        _gameManager = GameManager.Instance;
    }


    public void AddChips()
    {
        PointerController.HidePointers();

        _comparer.ClearStorage();

        Log.AddCommand(new AddChipsCommand());
    }


    public void ShuffleChips()
    {
        PointerController.HidePointers();

        _comparer.ClearStorage();

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
        _comparer.TryMatching(chip);
    }


#region PLACE ON BOARD

    public void DrawStartArray()
    {
        //DrawStartArrayAsync(GameManager.Instance.ChipsOnStartNumber).Forget();

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
                await UniTask.Delay(DelayOnDrawChips);
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
                await UniTask.Delay(DelayOnDrawChips);

                CameraController.Instance.MoveToBottomBound();
            }

            Chip newChip = DrawChip(chip.ShapeIndex, chip.ColorIndex, boardPos);

            added.Add(newChip);
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
                await UniTask.Delay(DelayOnDrawChips);

                CameraController.Instance.MoveToBottomBound();
            }

            chip.ChipFiniteStateMachine.SetSelfDestroyableState().Forget();

            ChipRegistry.CheckBoardCapacity();
        }
    }

#endregion


    private Chip DrawChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = Board.Instance[boardPosition.x, boardPosition.y];

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