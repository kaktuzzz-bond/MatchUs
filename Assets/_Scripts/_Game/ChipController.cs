using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
    public ChipRegistry ChipRegistry { get; private set; }

    private CommandLogger _commandLogger;

    private GameManager _gameManager;


    private void Awake()
    {
        ChipRegistry = new ChipRegistry();

        _commandLogger = new CommandLogger();

        _gameManager = GameManager.Instance;
    }


    public async UniTask AddChips()
    {
        //PointerController.HidePointers();

        var infos = ChipInfo.ExtractInfos(ChipRegistry.ActiveChips);

        var cloned = ChipInfo.GetClonedInfo(infos, ChipRegistry.Counter);

        await DrawArrayAsync(cloned);

        ChipComparer.ClearStorage();

        _commandLogger.AddCommand(new AddChipsCommand());
    }


    public void ShuffleChips()
    {
        //PointerController.HidePointers();

        ChipComparer.ClearStorage();

        _commandLogger.AddCommand(new ShuffleCommand());
    }


    public void ShowHints()
    {
        //PointerController.ShowHints();
    }


    public void UndoCommand()
    {
        _commandLogger.UndoCommand().Forget();
    }


    public void ProcessTappedChip(Chip chip)
    {
        var tapped = ChipComparer.HandleTap(chip);

        if (tapped == null)
        {
            Board.Instance.HideSelector();

            return;
        }

        if (tapped.Length == 1)
        {
            Board.Instance.ShowSelector(chip.transform.position);

            return;
        }

        // on matched
        Board.Instance.HideSelector();

        _commandLogger.AddCommand(new FadeOutCommand(tapped[0], tapped[1]));

        Board.Instance.CheckLines(tapped[0], tapped[1]);
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

        _commandLogger.CheckStackCount();
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
}