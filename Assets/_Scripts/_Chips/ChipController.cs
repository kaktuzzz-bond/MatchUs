using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
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


    public async UniTask DrawArrayAsync(List<ChipInfo> chipInfos)
    {
        int line = (int)chipInfos.First().position.y;

        foreach (ChipInfo info in chipInfos)
        {
            if ((int)info.position.y != line)
            {
                line = (int)info.position.y;

                await UniTask.Delay(100);
            }

            Chip chip = CreateChip(info);

            ChipRegistry.Register(chip);

            chip.Init(info);

            chip.PlaceOnBoardAsync().Forget();
        }

        await UniTask.Yield();

        Log.CheckStackCount();
    }


    //
    //
    // public async UniTask RemoveChipsAsync(List<Chip> chips)
    // {
    //     int line = chips[0].BoardPosition.y;
    //
    //     foreach (Chip chip in chips)
    //     {
    //         if (NextLine(ref line, chip.BoardPosition.y))
    //         {
    //             await UniTask.Delay(1000);
    //
    //             CameraController.Instance.MoveToBottomBound();
    //         }
    //
    //         //chip.ChipFiniteStateMachine.SetSelfDestroyableState().Forget();
    //
    //         ChipRegistry.CheckBoardCapacity();
    //     }
    // }


    private Chip CreateChip(ChipInfo info)
    {
        Transform instance = Instantiate(
                _gameManager.gameData.chipPrefab,
                info.position,
                Quaternion.identity,
                _gameManager.gameData.chipParent);

        if (!instance.TryGetComponent(out Chip chip)) return null;

        instance.name = $"Chip ({info.shapeIndex}, {info.colorIndex})";

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